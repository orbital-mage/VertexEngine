namespace VertexEngine.Common.Exceptions;

public class UninitializedException() : SystemException(DefaultMessage)
{
    private const string DefaultMessage = "GameWindow is not initialized.";
}