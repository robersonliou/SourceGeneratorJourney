using Microsoft.CodeAnalysis;

namespace SourceGeneratorLib.Generators.Base
{
    public abstract class AutoCanceledGenerator : ISourceGenerator
    {
        protected abstract void InitializeWithAutoCanceled(GeneratorInitializationContext context);
        protected abstract void ExecuteWithAutoCanceled(GeneratorExecutionContext context);
        
        public void Initialize(GeneratorInitializationContext context)
        {
            if (!context.CancellationToken.IsCancellationRequested)
                InitializeWithAutoCanceled(context);
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!context.CancellationToken.IsCancellationRequested) 
                ExecuteWithAutoCanceled(context);
        }

    }
}