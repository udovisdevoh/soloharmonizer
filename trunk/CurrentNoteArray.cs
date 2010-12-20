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
        private Dictionary<int, DateTime> rememberedNotes = new Dictionary<int, DateTime>();

        /// <summary>
        /// Remember whether note is on or off
        /// </summary>
        /// <param name="noteValue">note value (0 to 11)</param>
        /// <param name="isOn">whether note is on</param>
        internal void SetNote(int noteValue, bool isOn)
        {
            if (isOn)
            {
                if (rememberedNotes.ContainsKey(noteValue))
                {
                    rememberedNotes[noteValue] = DateTime.Now;
                }
                else
                {
                    rememberedNotes.Add(noteValue, DateTime.Now);
                }
            }
            else
            {
                rememberedNotes.Remove(noteValue);
            }
        }

        /// <summary>
        /// Whether note is on
        /// </summary>
        /// <param name="noteValue">note value (0 to 11)</param>
        /// <returns>whether note is on</returns>
        internal bool IsNoteOn(int noteValue)
        {
            return rememberedNotes.ContainsKey(noteValue) && DateTime.Now.Subtract(rememberedNotes[noteValue]).Milliseconds < 300;
        }
    }
}
