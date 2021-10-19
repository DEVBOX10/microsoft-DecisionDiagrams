﻿// <copyright file="VariableSet.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace DecisionDiagrams
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a set of variables.
    /// </summary>
    /// <typeparam name="T">The node type.</typeparam>
    public class VariableSet<T>
        where T : IDDNode
    {
        /// <summary>
        /// The set of variables.
        /// </summary>
        private HashSet<int> variables;

        /// <summary>
        /// Gets the largest index in the set.
        /// </summary>
        internal int MaxIndex { get; } = -1;

        /// <summary>
        /// Gets the manager object.
        /// </summary>
        public int ManagerId { get; }

        /// <summary>
        /// Gets the DD representing the variables for efficient
        /// caching and comparison purposes.
        /// </summary>
        public DDIndex AsIndex { get; private set; }

        /// <summary>
        /// Gets the variables in the set.
        /// </summary>
        public Variable<T>[] Variables { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableSet{T}"/> class.
        /// </summary>
        /// <param name="manager">The manager object.</param>
        /// <param name="variables">The variables.</param>
        internal VariableSet(DDManager<T> manager, Variable<T>[] variables)
        {
            this.ManagerId = manager.Uid;
            this.AsIndex = DDIndex.True;
            this.Variables = variables;
            this.variables = new HashSet<int>();

            for (int i = 0; i < variables.Length; i++)
            {
                var v = variables[i];
                for (int j = v.Indices.Length - 1; j >= 0; j--)
                {
                    var variableIndex = v.Indices[j];
                    this.MaxIndex = Math.Max(this.MaxIndex, variableIndex);
                    this.AsIndex = manager.And(this.AsIndex, manager.IdIdx(variableIndex));
                    this.variables.Add(variableIndex);
                }
            }
        }

        /// <summary>
        /// Does the variable set contain the variable.
        /// </summary>
        /// <param name="variable">The variable index.</param>
        /// <returns>Whether the set contains that variable.</returns>
        internal bool Contains(int variable)
        {
            return this.variables.Contains(variable);
        }
    }
}
