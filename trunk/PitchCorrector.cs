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
        private List<int> forbidenInvervalList;

        private Dictionary<int, List<int>> bindedNoteList;

        /// <summary>
        /// Create pitch corrector
        /// </summary>
        /// <param name="currentNoteArray">Remembers note being played</param>
        public PitchCorrector(CurrentNoteArray currentNoteArray)
        {
            this.currentNoteArray = currentNoteArray;

            forbidenInvervalList = new List<int>();
            forbidenInvervalList.Add(0);
            forbidenInvervalList.Add(1);
            forbidenInvervalList.Add(2);
            forbidenInvervalList.Add(6);
            forbidenInvervalList.Add(8);

            bindedNoteList = new Dictionary<int, List<int>>();
        }

        /// <summary>
        /// Try to harmonize pitch
        /// </summary>
        /// <param name="absolutePitch">pitch to fix</param>
        /// <returns>fixed absolute pitch or -1 if can't do anything about it</returns>
        internal int TryFixPitch(int absolutePitch)
        {
            int originalAbsolutePitch = absolutePitch;

            for (int i = 0; i < 10; i++)
            {
                foreach (int forbidenInterval in forbidenInvervalList)
                {
                    int relativePitch = absolutePitch % 12;

                    if (currentNoteArray.IsNoteOn((relativePitch + forbidenInterval) % 12) && absolutePitch - 1 != originalAbsolutePitch)
                    {
                        absolutePitch -= 1;
                    }
                    else if (currentNoteArray.IsNoteOn((relativePitch - forbidenInterval) % 12) && absolutePitch + 1 != originalAbsolutePitch)
                    {
                        absolutePitch += 1;
                    }
                }
            }

            foreach (int forbidenInterval in forbidenInvervalList)
            {
                int relativePitch = absolutePitch % 12;
                if (currentNoteArray.IsNoteOn((relativePitch + forbidenInterval) % 12))
                {
                    return -1;
                }
                else if (currentNoteArray.IsNoteOn((relativePitch - forbidenInterval) % 12))
                {
                    return -1;
                }
            }

            SetNoteBinding(originalAbsolutePitch, absolutePitch);

            return absolutePitch;
        }

        private void SetNoteBinding(int fromNote, int toNote)
        {
            List<int> bindedNoteListForNote;
            if (!bindedNoteList.TryGetValue(fromNote, out bindedNoteListForNote))
            {
                bindedNoteListForNote = new List<int>();
                bindedNoteList.Add(fromNote, bindedNoteListForNote);
            }
            bindedNoteListForNote.Add(toNote);
        }

        /// <summary>
        /// Get the list of notes that are binded to another note
        /// </summary>
        /// <param name="sourceNote">source note)</param>
        /// <returns>list of notes binded to it</returns>
        internal IEnumerable<int> GetBindedNoteList(int sourceNote)
        {
            List<int> bindedNoteListForNote;
            if (bindedNoteList.TryGetValue(sourceNote, out bindedNoteListForNote))
            {
                return bindedNoteListForNote;
            }
            else
            {
                return null;
            }
        }

        internal void ResetBindedNoteList(int sourceNote)
        {
            List<int> bindedNoteListForNote;
            if (bindedNoteList.TryGetValue(sourceNote, out bindedNoteListForNote))
            {
                bindedNoteListForNote.Clear();
            }
        }
    }
}
