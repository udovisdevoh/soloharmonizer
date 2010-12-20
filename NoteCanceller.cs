using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HarmonicSolo
{
    /// <summary>
    /// Cancel notes that are too close
    /// </summary>
    class NoteCanceller
    {
        /// <summary>
        /// Represents notes that are currently on
        /// </summary>
        private CurrentNoteArray currentNoteArray;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="currentNoteArray">current note array</param>
        public NoteCanceller(CurrentNoteArray currentNoteArray)
        {
            this.currentNoteArray = currentNoteArray;
        }

        /// <summary>
        /// Whether a note on side is on
        /// </summary>
        /// <param name="noteId">note id (0 to 11)</param>
        /// <returns></returns>
        internal bool IsNoteOnSideOn(int noteId)
        {
            switch (noteId)
            {
                case 0:
                    return currentNoteArray.IsNoteOn(11) || currentNoteArray.IsNoteOn(2);
                case 2:
                    return currentNoteArray.IsNoteOn(0) || currentNoteArray.IsNoteOn(4);
                case 4:
                    return currentNoteArray.IsNoteOn(2) || currentNoteArray.IsNoteOn(5);
                case 5:
                    return currentNoteArray.IsNoteOn(4) || currentNoteArray.IsNoteOn(7);
                case 7:
                    return currentNoteArray.IsNoteOn(5) || currentNoteArray.IsNoteOn(9);
                case 9:
                    return currentNoteArray.IsNoteOn(7) || currentNoteArray.IsNoteOn(11);
                case 11:
                    return currentNoteArray.IsNoteOn(9) || currentNoteArray.IsNoteOn(0);
            }
            return false;
        }
    }
}
