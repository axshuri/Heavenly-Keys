using System;
using System.Collections.Generic;
using USBUnlocker.UI;
using USBUnlocker.USB;
using USBUnlocker.Diagnostics;
using USBUnlocker.Network;
using USBUnlocker.Security;
using USBUnlocker.Reports;
using USBUnlocker.Utilities;

namespace USBUnlocker.Core
{
    public class Application
    {
        private readonly AppConfig _config;
        private readonly FeatureRegistry _registry;
        private readonly Dispatcher _dispatcher;
        private readonly HeaderEngine _headerEngine;
        private bool _running;
        
        public Application(AppConfig config)
        {
            _config = config;
            _registry = new FeatureRegistry(config);
            _dispatcher = new Dispatcher(_registry, config);
            _headerEngine = new HeaderEngine(config);
        }
        
        public void Initialize()
        {
            // Check administrator privileges
            if (!AdminHelper.IsAdministrator())
            {
                DisplayHelper.ShowElevationRequest();
                if (!AdminHelper.TryElevate())
                {
                    DisplayHelper.ShowAccessDenied();
                    Environment.Exit(4);
                }
            }
            
            // Detect Windows version
            SystemInfo.Detect();
            
            // Show loading screen
            _headerEngine.ShowLoadingScreen();
            
            // Register all capabilities (single source of truth)
            RegisterCapabilities();
            
            // Register explicit workflows
            RegisterWorkflows();
            
            // Auto-generate workflows from capabilities
            _registry.AutoGenerateWorkflows();
            
            // Check feature availability
            _registry.CheckAvailability();
            
            // Log startup
            Logger.Info("Heavenly Keys v3.0.0 started");
            Logger.Info($"Windows: {SystemInfo.OsVersion} ({SystemInfo.Architecture})");
            Logger.Info($"Computer: {SystemInfo.ComputerName}");
            Logger.Info($"Capabilities: {_registry.CapabilityCount} discovered, {_registry.AvailableCount} available");
            
            _running = true;
        }
        
        public void Run()
        {
            while (_running)
            {
                try
                {
                    // Display header with ASCII art
                    _headerEngine.DisplayHeader("MAIN MENU");
                    
                    // Build and display dynamic menu from registry
                    _registry.BuildMenu();
                    
                    // Get user input
                    Console.Write("\n Select an option: ");
                    string input = Console.ReadLine();
                    
                    if (string.IsNullOrEmpty(input))
                        continue;
                    
                    // Handle keyboard shortcuts
                    switch (input.ToUpper())
                    {
                        case "H":
                            ShowHelp();
                            break;
                        case "R":
                            Refresh();
                            break;
                        case "S":
                            ShowSearch();
                            break;
                        case "W":
                            ShowWorkflows();
                            break;
                        case "I":
                            ShowInspector();
                            break;
                        case "Q":
                        case "0":
                            if (ConfirmExit())
                                _running = false;
                            break;
                        default:
                            _dispatcher.Dispatch(input);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Menu error: " + ex.Message);
                    Console.WriteLine("\nAn error occurred, but USBUnlocker is still running.");
                    Console.WriteLine("Press any key to return to the Main Menu...");
                    Console.ReadKey();
                }
            }
            
            // Cleanup and exit
            Cleanup();
        }
        
        public void RunSilent()
        {
            try
            {
                // Run complete workflow
                var usbScanner = new UsbScanner(_config);
                var repairer = new UsbRepairer(_config);
                var verifier = new UsbVerifier(_config);
                var reportGen = new ReportGenerator(_config);
                
                // Scan
                Logger.Info("Starting USB scan...");
                var scanResult = usbScanner.Scan();
                
                // Repair
                if (_config.RepairEnabled)
                {
                    Logger.Info("Starting USB repair...");
                    repairer.Repair(scanResult);
                }
                
                // Verify
                Logger.Info("Verifying USB access...");
                var verifyResult = verifier.Verify();
                
                // Generate reports
                Logger.Info("Generating reports...");
                reportGen.GenerateAll();
                
                // Create ZIP
                reportGen.CreateZip();
                
                // Display summary
                DisplayHelper.ShowFinalSummary(scanResult, verifyResult);
                
                // Determine exit code
                int exitCode = DetermineExitCode(scanResult, verifyResult);
                Environment.Exit(exitCode);
            }
            catch (Exception ex)
            {
                Logger.Error("Silent workflow error: " + ex.Message);
                Environment.Exit(5);
            }
        }
        
        // =================================================================
        // CAPABILITY REGISTRATION — Single source of truth
        // =================================================================
        
        private void RegisterCapabilities()
        {
            // ── SYSTEM ──────────────────────────────────────────────
            _registry.Register(new Capability
            {
                Id = "SYSINFO",
                Name = "System Information",
                Category = "SYSTEM",
                Description = "Display detailed system information",
                Type = CapabilityType.Diagnostic,
                Action = () => { new SystemInfoDisplay().Show(); },
                AdminRequired = false,
                Order = 10
            });
            
            _registry.Register(new Capability
            {
                Id = "HWINFO",
                Name = "Hardware Information",
                Category = "SYSTEM",
                Description = "Display hardware inventory",
                Type = CapabilityType.Diagnostic,
                Action = () => { new HardwareInfoDisplay().Show(); },
                AdminRequired = false,
                Order = 11
            });
            
            _registry.Register(new Capability
            {
                Id = "DRIVERAUDIT",
                Name = "Driver Audit",
                Category = "SYSTEM",
                Description = "Audit installed device drivers",
                Type = CapabilityType.Diagnostic,
                Action = () => { new DriverAuditor().Audit(); },
                AdminRequired = false,
                Order = 12
            });
            
            _registry.Register(new Capability
            {
                Id = "SVCAUDIT",
                Name = "Service Audit",
                Category = "SYSTEM",
                Description = "Audit critical Windows services",
                Type = CapabilityType.Diagnostic,
                Action = () => { new ServiceAuditor().Audit(); },
                AdminRequired = false,
                Order = 13
            });
            
            _registry.Register(new Capability
            {
                Id = "INTEGRITY",
                Name = "Windows Integrity Check",
                Category = "SYSTEM",
                Description = "Check Windows system file integrity",
                Type = CapabilityType.Diagnostic,
                Action = () => { new IntegrityChecker().Check(); },
                AdminRequired = true,
                Risk = RiskLevel.Safe,
                Order = 14
            });
            
            // ── USB ─────────────────────────────────────────────────
            _registry.Register(new Capability
            {
                Id = "USBSCAN",
                Name = "USB Scan",
                Category = "USB",
                Description = "Scan USB configuration and policies",
                Type = CapabilityType.Diagnostic,
                Action = () => { new UsbScanner(_config).ScanAndDisplay(); },
                AdminRequired = false,
                Order = 20
            });
            
            _registry.Register(new Capability
            {
                Id = "USBINFO",
                Name = "USB Device Information",
                Category = "USB",
                Description = "Display USB device inventory",
                Type = CapabilityType.Diagnostic,
                Action = () => { new UsbDeviceEnumerator().Enumerate(); },
                AdminRequired = false,
                Order = 21
            });
            
            _registry.Register(new Capability
            {
                Id = "USBHISTORY",
                Name = "USB Device History",
                Category = "USB",
                Description = "View previously connected USB devices",
                Type = CapabilityType.Diagnostic,
                Action = () => { new UsbHistory().Show(); },
                AdminRequired = false,
                Order = 22
            });
            
            _registry.Register(new Capability
            {
                Id = "USBREPAIR",
                Name = "USB Scan and Repair",
                Category = "USB",
                Description = "Scan and repair local USB restrictions",
                Type = CapabilityType.Repair,
                Action = () => { new UsbRepairWorkflow(_config).Execute(); },
                AdminRequired = true,
                Risk = RiskLevel.Moderate,
                Dependencies = new List<string> { "USBSCAN" },
                Order = 23
            });
            
            _registry.Register(new Capability
            {
                Id = "USBVERIFY",
                Name = "Verify USB Access",
                Category = "USB",
                Description = "Verify current USB access status",
                Type = CapabilityType.Diagnostic,
                Action = () => { new UsbVerifier(_config).VerifyAndDisplay(); },
                AdminRequired = false,
                Dependencies = new List<string> { "USBSCAN" },
                Order = 24
            });
            
            // ── NETWORK ─────────────────────────────────────────────
            _registry.Register(new Capability
            {
                Id = "NETDIAG",
                Name = "Network Diagnostic",
                Category = "NETWORK",
                Description = "Perform read-only network diagnostics",
                Type = CapabilityType.Diagnostic,
                Action = () => { new NetworkDiagnostic().Run(); },
                AdminRequired = false,
                Order = 30
            });
            
            // ── SECURITY ────────────────────────────────────────────
            _registry.Register(new Capability
            {
                Id = "SECDIAG",
                Name = "Security Diagnostic",
                Category = "SECURITY",
                Description = "Display security configuration",
                Type = CapabilityType.Diagnostic,
                Action = () => { new SecurityDiagnostic().Run(); },
                AdminRequired = false,
                Order = 40
            });
            
            _registry.Register(new Capability
            {
                Id = "EVTLOG",
                Name = "Event Log Analysis",
                Category = "SECURITY",
                Description = "Analyze recent Windows events",
                Type = CapabilityType.Diagnostic,
                Action = () => { new EventLogAnalyzer().Analyze(); },
                AdminRequired = false,
                Order = 41
            });
            
            // ── REPORTS ─────────────────────────────────────────────
            _registry.Register(new Capability
            {
                Id = "GENREPORT",
                Name = "Generate Full Report",
                Category = "REPORTS",
                Description = "Generate comprehensive diagnostic report",
                Type = CapabilityType.Report,
                Action = () => { new ReportGenerator(_config).GenerateAll(); },
                AdminRequired = false,
                Order = 50
            });
            
            _registry.Register(new Capability
            {
                Id = "JSONEXPORT",
                Name = "Export JSON",
                Category = "REPORTS",
                Description = "Export diagnostic data as JSON",
                Type = CapabilityType.Export,
                Action = () => { new JsonExporter(_config).Export(); },
                AdminRequired = false,
                Order = 51
            });
            
            _registry.Register(new Capability
            {
                Id = "ZIPREPORT",
                Name = "Create ZIP Report",
                Category = "REPORTS",
                Description = "Create ZIP archive of reports",
                Type = CapabilityType.Export,
                Action = () => { new ReportGenerator(_config).CreateZip(); },
                AdminRequired = false,
                Order = 52
            });
            
            _registry.Register(new Capability
            {
                Id = "VIEWLOG",
                Name = "View Repair Log",
                Category = "REPORTS",
                Description = "Display the repair log file",
                Type = CapabilityType.Utility,
                Action = () => { Logger.DisplayLog(); },
                AdminRequired = false,
                Order = 53
            });
            
            // ── TOOLS ───────────────────────────────────────────────
            _registry.Register(new Capability
            {
                Id = "COMPLETE",
                Name = "Complete Diagnostic",
                Category = "TOOLS",
                Description = "Run full system diagnostic suite",
                Type = CapabilityType.Workflow,
                Action = () => { new CompleteDiagnostic(_config).Execute(); },
                AdminRequired = true,
                Order = 60
            });
            
            _registry.Register(new Capability
            {
                Id = "DRYRUN",
                Name = "Dry Run / Simulation",
                Category = "TOOLS",
                Description = "Detect issues without making changes",
                Type = CapabilityType.Tool,
                Action = () => { new DryRunner(_config).Run(); },
                AdminRequired = false,
                Risk = RiskLevel.Safe,
                Order = 61
            });
            
            _registry.Register(new Capability
            {
                Id = "ROLLBACK",
                Name = "Rollback Last Repair",
                Category = "TOOLS",
                Description = "Restore previous USB configuration",
                Type = CapabilityType.Repair,
                Action = () => { new RollbackManager(_config).Rollback(); },
                AdminRequired = true,
                Risk = RiskLevel.Moderate,
                Order = 62
            });
            
            // ── CONTEXT ACTIONS (only shown when relevant) ───────────
            _registry.Register(new Capability
            {
                Id = "CTX_REPAIR",
                Name = ">> Quick Repair (from last scan)",
                Category = "ACTIONS",
                Description = "Repair issues found in last scan",
                Type = CapabilityType.Repair,
                Action = () => { new UsbRepairWorkflow(_config).Execute(); },
                AdminRequired = true,
                Risk = RiskLevel.Moderate,
                IsContextAction = true,
                ContextCondition = () => _registry.GetLastScanHadIssues(),
                Order = 70
            });
            
            _registry.Register(new Capability
            {
                Id = "CTX_VERIFY",
                Name = ">> Verify Last Repair",
                Category = "ACTIONS",
                Description = "Verify the last repair operation",
                Type = CapabilityType.Diagnostic,
                Action = () => { new UsbVerifier(_config).VerifyAndDisplay(); },
                AdminRequired = false,
                IsContextAction = true,
                ContextCondition = () => _registry.GetLastScanHadRepair(),
                Order = 71
            });
        }
        
        // =================================================================
        // EXPLICIT WORKFLOWS
        // =================================================================
        
        private void RegisterWorkflows()
        {
            // Manual: Complete USB Recovery
            _registry.RegisterWorkflow(new Workflow
            {
                Id = "WF_USB_RECOVERY",
                Name = "USB Recovery Workflow",
                Description = "Full USB recovery: scan, backup, repair, verify",
                AdminRequired = true,
                Risk = RiskLevel.Moderate,
                Order = 200,
                Steps = new List<WorkflowStep>
                {
                    new WorkflowStep { CapabilityId = "USBSCAN", Description = "Scan USB configuration" },
                    new WorkflowStep { CapabilityId = "DRYRUN", Description = "Dry run to preview changes" },
                    new WorkflowStep { CapabilityId = "USBREPAIR", Description = "Repair USB restrictions", RequiresConfirmation = true },
                    new WorkflowStep { CapabilityId = "USBVERIFY", Description = "Verify USB access" },
                    new WorkflowStep { CapabilityId = "GENREPORT", Description = "Generate report" }
                }
            });
            
            // Manual: System Health Check
            _registry.RegisterWorkflow(new Workflow
            {
                Id = "WF_HEALTH_CHECK",
                Name = "System Health Check",
                Description = "Quick system health overview: info, drivers, network, security",
                Risk = RiskLevel.Safe,
                Order = 201,
                Steps = new List<WorkflowStep>
                {
                    new WorkflowStep { CapabilityId = "SYSINFO", Description = "System Information" },
                    new WorkflowStep { CapabilityId = "HWINFO", Description = "Hardware Information" },
                    new WorkflowStep { CapabilityId = "NETDIAG", Description = "Network Diagnostic" },
                    new WorkflowStep { CapabilityId = "SECDIAG", Description = "Security Diagnostic" }
                }
            });
        }
        
        // =================================================================
        // UI ACTIONS
        // =================================================================
        
        private void ShowHelp()
        {
            _headerEngine.DisplayHeader("HELP");
            _registry.DisplayHelp();
            DisplayHelper.PauseReturnToMenu();
        }
        
        private void ShowSearch()
        {
            _headerEngine.DisplayHeader("SEARCH");
            Console.Write("\n Enter search keyword: ");
            string query = Console.ReadLine();
            _registry.Search(query);
            DisplayHelper.PauseReturnToMenu();
        }
        
        private void ShowWorkflows()
        {
            _headerEngine.DisplayHeader("WORKFLOWS");
            _registry.DisplayWorkflows();
            DisplayHelper.PauseReturnToMenu();
        }
        
        private void ShowInspector()
        {
            _headerEngine.DisplayHeader("CAPABILITY INSPECTOR");
            _registry.DisplayInspector();
            DisplayHelper.PauseReturnToMenu();
        }
        
        private void Refresh()
        {
            _registry.CheckAvailability();
            _registry.AutoGenerateWorkflows();
            SystemInfo.Detect();
            Console.WriteLine("\nMenu refreshed. Capabilities re-scanned.");
            Console.WriteLine($"Available: {_registry.AvailableCount}/{_registry.CapabilityCount}");
            DisplayHelper.PauseReturnToMenu();
        }
        
        private bool ConfirmExit()
        {
            Console.WriteLine("\n============================================================");
            Console.WriteLine("                         EXIT");
            Console.WriteLine("============================================================");
            Console.WriteLine("\nAre you sure you want to exit USBUnlocker?");
            Console.WriteLine("\n[Y] Yes");
            Console.WriteLine("[N] No");
            Console.Write("\nChoice: ");
            
            string input = Console.ReadLine();
            return input != null && input.ToUpper() == "Y";
        }
        
        private void Cleanup()
        {
            Logger.Info("Cleaning up temporary files...");
            TempManager.Cleanup(_config);
        }
        
        private int DetermineExitCode(UsbScanResult scan, UsbVerifyResult verify)
        {
            if (verify.AllBlocked)
                return 1;
            if (scan.DomainDetected)
                return 2;
            if (scan.ExternalDetected)
                return 3;
            return 0;
        }
    }
}
