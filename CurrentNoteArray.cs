using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HarmonicSolo
{
    /// <summary>
    /// Represents notes that are currently on
    /// </summary>
    class CurrentNoteArray
    {
        private HashSet<int> rememberedNotes = new HashSet<int>();

        /// <summary>
        /// Remember whether note is on or off
        /// </summary>
        /// <param name="noteValue">note value (0 to 11)</param>
        /// <param name="isOn">whether note is on</param>
        internal void SetNote(int noteValue, bool isOn)
        {
            if (isOn)
            {
                rememberedNotes.Add(noteValue % 12);
            }
            else
            {
                rememberedNotes.Remove(noteValue % 12);
            }
        }

        /// <summary>
        /// Whether note is on
        /// </summary>
        /// <param name="noteValue">note value (0 to 11)</param>
        /// <returns>whether note is on</returns>
        internal bool IsNoteOn(int noteValue)
        {
            return rememberedNotes.Contains(noteValue % 12);
        }
    }
}
