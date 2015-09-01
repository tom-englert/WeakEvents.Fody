﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace WeakEvents.Fody.IlEmit
{
    // IL is a stack based programming language based on low level primitives.
    // This interface, its implementations and extension methods attempt to
    // make IL emitting safer (by enforcing some type safety) and easier
    // (by taking care of ordering IL and making code more readable).
    //
    // They emitters are designed to be chained together, ala LINQ.
    interface IlEmitter
    {
        // Call this to generate the instructions
        IEnumerable<Instruction> Emit();

        // The method the instructions are for.
        MethodDefinition Method { get; }
    }

    // Base emitter that encapsulates common concepts
    abstract class IlEmitterBase  : IlEmitter
    {
        IlEmitter _preceedingCode;

        // Code is linear: each emitter must have a preceeding emitter
        protected IlEmitterBase(IlEmitter preceedingCode)
        {
            _preceedingCode = preceedingCode; 
        }

        public abstract IEnumerable<Instruction> Emit();

        // Called by derived classes as part of their Emit() method.
        protected IEnumerable<Instruction> EmitPreceeding()
        {
            return _preceedingCode.Emit();
        }

        // Chain the method definition.
        public MethodDefinition Method { get { return _preceedingCode.Method; } }
    }
}
