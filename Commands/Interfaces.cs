namespace Dec.DiscordIPC.Commands {
    public interface ICommand {
        string cmd { get; }
        string nonce { get; set; }
    }

    public interface ICommandArgs {
    }

    public interface IResponse {
        string cmd { get; set; }
        string nonce { get; set; }
    }

    public interface IResponseData {
    }
}
