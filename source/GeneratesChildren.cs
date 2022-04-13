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
    /// Interface used for Tree structures to easily generate children of the current node.
    /// </summary>
    /// <typeparam name="T">Type of the generated children.</typeparam>
    interface GeneratesChildren<T>
    {
        LinkedList<T> GenerateChildren();
    }
}
