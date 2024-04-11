﻿namespace SynthesizerLibrary.Util;

public static class NoteHelper
{
    // Defines the sequence of notes, considering sharps but calculating flats.
    private static readonly string[] Notes = { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };

    public static (int NoteIndex, int Octave) ParseNoteString(string noteString)
    {
        // Validate the noteString.
        if (string.IsNullOrWhiteSpace(noteString) || noteString.Length < 2)
        {
            throw new ArgumentException("Invalid note string.");
        }

        // Extract the note and octave from the noteString.
        var notePart = noteString[..^1];
        var octaveChar = noteString[^1];

        // Validate the octave part.
        if (!char.IsDigit(octaveChar))
        {
            throw new ArgumentException("Invalid octave.");
        }
        var octave = int.Parse(octaveChar.ToString());

        // Handle flats by converting them to their sharp equivalents.
        if (notePart.EndsWith("b") && notePart.Length > 1)
        {
            var baseNote = notePart[0];
            var baseNoteIndex = Array.IndexOf(Notes, baseNote.ToString()) - 1;
            if (baseNoteIndex < 0) baseNoteIndex += Notes.Length; // Wrap around for 'A' flat.
            notePart = Notes[baseNoteIndex];
        }

        // Find the index of the note.
        var noteIndex = Array.IndexOf(Notes, notePart);
        if (noteIndex == -1)
        {
            throw new ArgumentException("Invalid note.");
        }

        return (noteIndex, octave);
    }

    public static double NoteToFrequency(int noteIndex, int octave, double referenceFrequency)
    {
        // Calculate the number of half steps n from A4
        int halfStepsFromA0 = octave * 12 + noteIndex;

        // Use the formula to calculate the frequency
        double frequency = referenceFrequency * Math.Pow(2, halfStepsFromA0 / 12.0);
        return frequency;
    }
}