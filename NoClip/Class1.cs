using System;
using System.Collections.Generic;
using System.Text;
using InfinityScript;

namespace NoClip
{
    public class NoClip : BaseScript
    {

        public NoClip()
            : base()
        {
            CreateServerHUD();            
            PlayerConnected += new Action<Entity>(player =>
            {

                player.Call("notifyonplayercommand", "fly", "+actionslot 1");
                player.Call("notifyonplayercommand", "flyb", "+actionslot 2");
                player.OnNotify("fly", (ent) =>
                {
                    if (player.GetField<string>("sessionstate") != "spectator")
                    {
                        player.Call("allowspectateteam", "freelook", true);
                        player.SetField("sessionstate", "spectator");
                        player.Call("setcontents", 0);
                    }
                    else
                    {
                        player.Call("allowspectateteam", "freelook", false);
                        player.SetField("sessionstate", "playing");
                        player.Call("setcontents", 100);
                    }
                });
                player.OnNotify("flyb", (ent) =>
                {
                    Utilities.RawSayTo(player, player.Origin.X + ", " + player.Origin.Y + ", " + player.Origin.Z);
                    Log.Write(LogLevel.All, "AddItemSpawn(" + player.Origin.X + "F, " + player.Origin.Y + "F, " + player.Origin.Z + "F);");
                });
                player.SpawnedPlayer += new Action(() =>
                {
                    player.Call("setplayerdata",new Parameter[] { 
                        "killstreaksState",
                        "count",
                        4});
                    player.Call("setplayerdata", new Parameter[] { 
                        "killstreaksState",
                        "count",
                        6});
                    player.Call("setplayerdata", new Parameter[] { 
                        "killstreaksState",
                        "count",
                        8});
                });
            });
        }

        private void CreateServerHUD()
        {
            HudElem WeaponSwitch;
            WeaponSwitch = HudElem.CreateServerFontString("default", 1.5f);
            WeaponSwitch.SetPoint("center", "center", -20, 230);
            WeaponSwitch.HideWhenInMenu = true;
            WeaponSwitch.Archived = false;
            WeaponSwitch.SetText("^2Press ^1[{+actionslot 1}] ^2for NoClip!^2Press ^1[{+actionslot 2}] ^2for save Pos!");
        }

        public override EventEat OnSay3(Entity player, ChatType type, string name, ref string message)
        {
            if (message.EndsWith("!pos"))
            {
                Utilities.RawSayTo(player, player.Origin.X + ", " + player.Origin.Y + ", " + player.Origin.Z);
                Log.Write(LogLevel.All, player.Name.ToString() + ": " + player.Origin.X + ", " + player.Origin.Y + ", " + player.Origin.Z);
                return EventEat.EatGame;
            }
            
            if (message.Equals("!getmodell"))
            {

                Entity test = MapObjects.CreateObject(player.Origin, "mp_body_opforce_ghillie_desert_sniper");
                return EventEat.EatGame;
            }
            return EventEat.EatNone;
        }
    }
}
