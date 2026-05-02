#if !NET8_0_OR_GREATER
namespace System.Runtime.CompilerServices;

internal sealed class CollectionBuilderAttribute : Attribute
{
    public CollectionBuilderAttribute(Type builderType, string methodName)
    {
        BuilderType = builderType;
        MethodName = methodName;
    }

    public Type BuilderType { get; }
    public string MethodName { get; }
}
#endif
