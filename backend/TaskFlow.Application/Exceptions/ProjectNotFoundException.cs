


namespace TaskFlow.Application.Exceptions;

public sealed class ProjectNotFoundException : Exception{
    public ProjectNotFoundException()
        : base("Projeto não encontrado.")
    {
    }
}



