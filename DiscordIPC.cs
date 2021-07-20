using Dec.DiscordIPC.Commands;
using Dec.DiscordIPC.Core;
using Dec.DiscordIPC.Events;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dec.DiscordIPC {
    public class DiscordIPC : LowLevelDiscordIPC {

        public DiscordIPC(string clientId) : base(clientId) { }

        #region Commands

        public async Task<Authorize.Data> SendCommandAsync(Authorize.Args args) =>
            await SendCommandAsync_Core("AUTHORIZE", args) as Authorize.Data;

        public async Task<Authenticate.Data> SendCommandAsync(Authenticate.Args args) =>
            await SendCommandAsync_Core("AUTHENTICATE", args) as Authenticate.Data;

        private async Task<dynamic> SendCommandAsync_Core(string cmd, dynamic args) {
            var nonce = Guid.NewGuid().ToString();
            dynamic payload = new {
                cmd,
                nonce,
                args
            };

            JsonElement response = await SendCommandWeakTypeAsync(payload);
            return response.GetProperty("data").ToObject<Authorize.Data>();
        }

        #endregion

        #region Events

        public async Task SubscribeAsync(GuildStatus.Args args) =>
            await SubscribeAsync_Core("GUILD_STATUS", args);

        public async Task SubscribeAsync(MessageCreate.Args args) =>
            await SubscribeAsync_Core("MESSAGE_CREATE", args);

        private async Task SubscribeAsync_Core(string evt, dynamic args) {
            var nonce = Guid.NewGuid().ToString();
            dynamic payload = new {
                cmd = "SUBSCRIBE",
                nonce,
                evt,
                args
            };

            await SendCommandWeakTypeAsync(payload);
            await messageReadLoop.WaitForResponse(nonce);
        }

        #endregion
    }
}
