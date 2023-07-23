# SuperScan
EXILED plugin that performs a "scan" of alive entities.

## Installation
Download SuperScan.dll from [Releases](/Releases).

Move SuperScan.dll to .config/EXILED/Plugins and restart server.

## Configuration
Edit values in .config/EXILED/Configs/PORT-config.yml

Example config with default values:
```yml
super_scan:
# Is the Plugin enabled.
  is_enabled: true
  # Debug mode.
  debug: false
  # Scan interval in seconds.
  scan_interval: 300
  # Scan duration in seconds.
  scan_duration: 20
  # C.A.S.S.I.E. voicelines. Result will always be suffixed with scan results.
  cassie_lines:
    pre-scan: .G3 Execute area .G6 anomaly scan protocol
    result: '.G3 Scan complete . containment .G4 breach confirmed . detected . '
```
* is_enabled
> A boolean; Controls if SCP_Kick is enabled or not.
* debug
> A boolean; Enables some extra logging.
* scan_interval
> An int; Scan interval in seconds.
* scan_duration
> An int; Scan duration in seconds.
* cassie_lines
> A dictionary with strings; Custom Cassie voicelines.
