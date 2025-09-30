using System.Net;

namespace Tenisu.Common;

public record Error(string Message, Enum Code, HttpStatusCode StatusCode);