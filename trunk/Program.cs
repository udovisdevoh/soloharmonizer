﻿using System;
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
                int newPitch = pitchCorrector.TryFixPitch(message.Data1);
                if (newPitch != -1)
                {
                    message = new ChannelMessage(message.Command, message.MidiChannel, newPitch, message.Data2);
                    currentNoteArray.SetNote(message.Data1 % 12, true);
                    outputDevice.Send(message);
                }
            }
            else if (message.Command == ChannelCommand.NoteOff || (message.Command == ChannelCommand.NoteOn && message.Data2 < 1)) //note off
            {
                IEnumerable<int> bindedNoteList = pitchCorrector.GetBindedNoteList(message.Data1);
                if (bindedNoteList != null)
                    foreach (int bindedNote in new List<int>(bindedNoteList))
                    {
                        currentNoteArray.SetNote(bindedNote % 12, false);
                        outputDevice.Send(new ChannelMessage(ChannelCommand.NoteOff, message.MidiChannel, bindedNote, 0));
                    }

                pitchCorrector.ResetBindedNoteList(message.Data1);

                /*currentNoteArray.SetNote(message.Data1 % 12, false);
                outputDevice.Send(message);*/
            }
        }
    }
}
