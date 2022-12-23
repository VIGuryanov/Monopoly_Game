using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XProtocol;
using XProtocol.Packets.Client_Server;
using XProtocol.Serializator;

namespace MonopolyMAUI.Server
{
    internal static class DialogueMethods
    {
        internal static XClient client;

        internal static async Task SendMyNickname(string nickname)
        {
            client.QueuePacketSend(
                XPacketConverter.Serialize(
                    XPacketType.Nickname,
                    NickNamePacket.EncodeString(nickname))//{ Nicknames = new NickNames { Nicks = new[] { nickname } } })
                    .ToPacket());
        }

        internal static async Task<string[]> GetTeamNickNames()
        {
            while (!DialogueFields.ApproveGameRequested)
                Thread.Sleep(2000);

            DialogueFields.ApproveGameRequested = false;

            var memory = DialogueFields.PlayersNicks;
            DialogueFields.PlayersNicks = null;
            return memory.ToArray();
        }
    }
}
