﻿using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace WeakEvents.Fody.IlEmit.StandardIl
{
    // OpCodes.Ldftn
    class EmitLoadMethod : IlEmitterBase
    {
        private readonly MethodReference _targetMethod;

        public EmitLoadMethod(IlEmitter preceedingCode, MethodReference targetMethod)
            : base(preceedingCode)
        {
            _targetMethod = targetMethod;
        }

        public override IEnumerable<Instruction> Emit()
        {
            if (Method.IsStatic)
            {
                return EmitPreceeding().Concat(new[] { Instruction.Create(OpCodes.Ldnull), Instruction.Create(OpCodes.Ldftn, _targetMethod) });
            }

            return EmitPreceeding().Concat(new[] { Instruction.Create(OpCodes.Ldarg_0), Instruction.Create(OpCodes.Ldftn, _targetMethod) });
        }
    }

    static partial class EmitterExtensions
    {
        public static IlEmitter LoadMethod(this IlEmitter preceedingCode, MethodReference targetMethod)
        {
            return new EmitLoadMethod(preceedingCode, targetMethod);
        }
        public static IlEmitter LoadMethod(this MethodDefinition method, MethodReference targetMethod)
        {
            return LoadMethod(new EmptyEmitter(method), targetMethod);
        }
    }
}
