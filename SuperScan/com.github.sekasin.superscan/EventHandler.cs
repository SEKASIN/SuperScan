using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using Server = Exiled.Events.Handlers.Server;
using MEC;
using PlayerRoles;
using Cassie = Exiled.API.Features.Cassie;

namespace SuperScan.com.github.sekasin.superscan
{
    public class EventHandler
    {
        private readonly Plugin<Config> _main;
        private readonly bool _debugMode;
        private CoroutineHandle timer;
        private CoroutineHandle timer2;
        private readonly String prescan;
        private readonly String result;
        private readonly int scanInterval;
        private readonly int scanDuration;
        
        
        public EventHandler(Plugin<Config> plugin) {
            _main = plugin;
            _debugMode = plugin.Config.Debug;
            if (_debugMode) {
                Log.Info("Loading EventHandler");
            }

            scanInterval = plugin.Config.ScanInterval;
            scanDuration = plugin.Config.ScanDuration;
            prescan = plugin.Config.CassieLines.FirstOrDefault(x => x.Key == "pre-scan").Value;
            result = plugin.Config.CassieLines.FirstOrDefault(x => x.Key == "result").Value;
            
            Server.RoundStarted += StartScanTimer;
            Server.RestartingRound += StopScanTimer;
            Server.RoundEnded += StopScanTimer;
        }

        private void Scan() {
            int[] roles = { 0, 0, 0, 0, 0, 0 };
            //SCP, Scientist, FacilityGuard, NTF, ClassD, Chaos
            List<Player> alivePlayers = new List<Player>();
            foreach (Player player in Player.List) { if (player.IsAlive && !player.IsNPC) alivePlayers.Append(player); }
            if (alivePlayers.Count == 0) {
                Timing.KillCoroutines(timer);
                Timing.KillCoroutines(timer2);
                StartScanTimer();
                return;
            }
            Cassie.Clear();
            Cassie.Message(prescan, true, false, true);
            foreach (Player player in alivePlayers) {
                switch (player.Role.Side) {
                    case Side.Scp:
                        roles[0]++;
                        break;
                    case Side.Mtf:
                        if (player.Role.Type == RoleTypeId.Scientist) {
                            roles[1]++;
                        } else if (player.Role.Type == RoleTypeId.FacilityGuard) {
                            roles[2]++;
                        } else {
                            roles[3]++;
                        }
                        break;
                    case Side.ChaosInsurgency:
                        if (player.Role.Type == RoleTypeId.ClassD) {
                            roles[4]++;
                        } else {
                            roles[5]++;
                        }
                        break;
                }
            }

            timer2 = Timing.CallDelayed(scanDuration, () => {
                //SCP, Scientist, FacilityGuard, NTF, ClassD, Chaos
                Cassie.Message(
                    result
                    + (roles[0] > 0 ? _pluraller(roles[0], "scpsubject") : "")
                    + (roles[1] > 0 ? _pluraller(roles[1], "scientist") : "")
                    + (roles[2] > 0 ? _pluraller(roles[2], "facility guard") : "")
                    + (roles[3] > 0 ? roles[3] + " ninetailedfox . " : "")
                    + (roles[4] > 0 ? _pluraller(roles[4], "classd") : "")
                    + (roles[5] > 0 ? _pluraller(roles[5], "chaosinsurgency operative") : "") + " .G1 .G2 ",
                    true, false, true);
                Timing.KillCoroutines(timer);
                Timing.KillCoroutines(timer2);
                StartScanTimer();
            });
        }

        private String _pluraller(int count, string basestr) {
            return count == 1 ? "1 " + basestr + " . " : count + " " + basestr + "s . ";
        }
        
        private void StartScanTimer() {
            timer = Timing.CallDelayed(scanInterval, Scan);
        }

        private void StopScanTimer() {
            Timing.KillCoroutines(timer);
            Timing.KillCoroutines(timer2);
        }

        private void StopScanTimer(RoundEndedEventArgs ev) {
            StopScanTimer();
        }

        public void UnregisterEvents() {
            StopScanTimer();
            Server.RoundStarted -= StartScanTimer;
            Server.RestartingRound -= StopScanTimer;
            Server.RoundEnded -= StopScanTimer;
        }
    }
}