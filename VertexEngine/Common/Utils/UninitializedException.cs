namespace VertexEngine.Common.Utils;

public class UninitializedException() : SystemException(DefaultMessage)
{
    private const string DefaultMessage = "GameWindow is not initialized.";
}