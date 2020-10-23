using System;
namespace AIActions
{
    public interface GeneratedAction
    {
        float Score(AISkin data);
        GeneratedAction Generate(AISkin skin);
    }
}
