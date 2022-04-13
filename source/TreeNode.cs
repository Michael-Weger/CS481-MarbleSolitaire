using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// CS 481 AI
// mweger

namespace MarbleSolitaire
{
    /// <summary>
    /// A tree node for generic types which can generate child nodes.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the node.</typeparam>
    class TreeNode<T> where T : GeneratesChildren<T>
    {
        private T m_Data; // Node data
        private TreeNode<T> m_Parent; // Parent node
        private LinkedList<TreeNode<T>> m_Children; // Child nodes
        private int m_Depth; // Depth in the tree

        /// <summary>
        /// Generates a tree node with no parent. Usually the head node of a tree.
        /// </summary>
        /// <param name="data">The data to store on the node.</param>
        public TreeNode(T data)
        {
            this.m_Data = data;
            this.m_Parent = null;
            this.m_Depth = 0;
            this.m_Children = new LinkedList<TreeNode<T>>();

            foreach (T child in data.GenerateChildren())
            {
                m_Children.AddLast(new TreeNode<T>(child, this));
            }
        }

        /// <summary>
        /// Generates a tree node with the specified parent.
        /// </summary>
        /// <param name="data">The data to store on the node.</param>
        /// <param name="parent">The parent node.</param>
        public TreeNode(T data, TreeNode<T> parent)
        {
            this.m_Data = data;
            this.m_Parent = parent;
            this.m_Depth = this.m_Parent.Depth + 1;
            this.m_Children = new LinkedList<TreeNode<T>>();

            foreach (T child in data.GenerateChildren())
            {
                m_Children.AddLast(new TreeNode<T>(child, this));
            }
        }

        /// <summary>
        /// Prints all ancestor nodes.
        /// </summary>
        public void PrintAncestors()
        {
            if (this.m_Parent == null)
            {
                Console.WriteLine("Depth: " + this.m_Depth + "\n" + this.m_Data.ToString());
            }
            else
            {
                this.m_Parent.PrintAncestors();
                Console.WriteLine("Depth: " + this.m_Depth + "\n" + this.m_Data.ToString());
            }
        }
        /// <summary>
        /// Node data accessor.
        /// </summary>
        public T Data
        {
            get
            {
                return m_Data;
            }
        }

        /// <summary>
        /// Node parent accessor.
        /// </summary>
        public TreeNode<T> Parent
        {
            get
            {
                return m_Parent;
            }
        }

        /// <summary>
        /// Node depth accessor. 
        /// </summary>
        public int Depth
        {
            get
            {
                return m_Depth;
            }
        }

        /// <summary>
        /// Node children accessor.
        /// </summary>
        public LinkedList<TreeNode<T>> Children
        {
            get
            {
                return m_Children;
            }
        }
    }
}
