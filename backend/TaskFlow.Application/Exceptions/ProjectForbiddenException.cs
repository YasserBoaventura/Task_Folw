using System;

namespace TaskFlow.Application.Exceptions;

public class ProjectForbiddenException : Exception{

    public ProjectForbiddenException()
        : base("Não tens permissão para realizar esta operação.")
    {
    }
  }


