using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

namespace HarmonicSolo
{
    class Program
    {
        private static InputDevice inputDevice;

        private static OutputDevice outputDevice;

        private static CurrentNoteArray currentNoteArray;

        private static PitchCorrector pitchCorrector;

        static void Main(string[] args)
        {
            inputDevice = new InputDevice(1);
            outputDevice = new OutputDevice(1);
            currentNoteArray = new CurrentNoteArray();
            pitchCorrector = new PitchCorrector(currentNoteArray);

            inputDevice.StartRecording();

            inputDevice.ChannelMessageReceived += new EventHandler<ChannelMessageEventArgs>(inputDevice_ChannelMessageReceived);
            Console.ReadLine();

            inputDevice.StopRecording();

            outputDevice.Dispose();
            inputDevice.Dispose();
        }

        static void inputDevice_ChannelMessageReceived(object sender, ChannelMessageEventArgs e)
        {
            ChannelMessage message = e.Message;

            if (message.Command == ChannelCommand.NoteOn && message.Data2 >= 1)
            {
                currentNoteArray.SetNote(message.Data1, true);

                IEnumerable<int> listNoteToTurnOff = pitchCorrector.GetListNoteToTurnOff(message.Data1);
                try
                {
                    foreach (int noteToTurnOff in new List<int>(listNoteToTurnOff))
                        outputDevice.Send(new ChannelMessage(ChannelCommand.NoteOff, message.MidiChannel, noteToTurnOff, 0));
                }
                catch (Exception exception)
                {
                    //sorry but we need some fault tollerance here
                }
            }
            else if (message.Command == ChannelCommand.NoteOff || (message.Command == ChannelCommand.NoteOn && message.Data2 < 1)) //note off
            {
                currentNoteArray.SetNote(message.Data1, false);
            }

            outputDevice.Send(message);
        }
    }
}
