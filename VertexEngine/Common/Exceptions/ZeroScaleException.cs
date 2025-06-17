namespace VertexEngine.Common.Exceptions;

public class ZeroScaleException() : SystemException(DefaultMessage)
{
    private const string DefaultMessage = "Cannot set Transform Scale to zero.";
}