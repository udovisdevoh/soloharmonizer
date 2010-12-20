using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HarmonicSolo
{
    /// <summary>
    /// Changes the pitch of a note to harmonize it
    /// </summary>
    class PitchCorrector
    {
        /// <summary>
        /// Remembers note being played
        /// </summary>
        private CurrentNoteArray currentNoteArray;

        /// <summary>
        /// List of intervals to avoid
        /// </summary>
        private List<int> forbiddenInvervalList;

        private List<int> listNoteToTurnOff;

        /// <summary>
        /// Create pitch corrector
        /// </summary>
        /// <param name="currentNoteArray">Remembers note being played</param>
        public PitchCorrector(CurrentNoteArray currentNoteArray)
        {
            listNoteToTurnOff = new List<int>();

            this.currentNoteArray = currentNoteArray;

            forbiddenInvervalList = new List<int>();
            forbiddenInvervalList.Add(1);
            forbiddenInvervalList.Add(2);
            forbiddenInvervalList.Add(6);
        }

        internal IEnumerable<int> GetListNoteToTurnOff(int currentNote)
        {
            listNoteToTurnOff.Clear();
            for (int pitch = 0; pitch < 128; pitch++)
            {
                foreach (int forbiddenInterval in forbiddenInvervalList)
                {
                    if ((currentNote + forbiddenInterval) % 12 == pitch % 12 || (currentNote - forbiddenInterval) % 12 == pitch % 12)
                    {
                        listNoteToTurnOff.Add(pitch);
                    }
                }
            }
            return listNoteToTurnOff;
        }
    }
}
