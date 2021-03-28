using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Capsaicin.CodeAnalysis.Extensions
{
    internal class AttributeDataMock : AttributeData
    {
        public AttributeDataMock(INamedTypeSymbol? commonAttributeClass)
        {
            CommonAttributeClass = commonAttributeClass;
        }
        protected override INamedTypeSymbol? CommonAttributeClass { get; }

        protected override IMethodSymbol? CommonAttributeConstructor => throw new NotImplementedException();

        protected override SyntaxReference? CommonApplicationSyntaxReference => throw new NotImplementedException();

        protected override ImmutableArray<TypedConstant> CommonConstructorArguments => throw new NotImplementedException();

        protected override ImmutableArray<KeyValuePair<string, TypedConstant>> CommonNamedArguments => throw new NotImplementedException();
    }
}