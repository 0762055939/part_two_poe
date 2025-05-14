using System.IO;
using System.Media;
using System;

namespace part_two_poe
{
    public class AudioPlayer
    {
        // Constructor

        // Constructor for the 'voice_greeting' class
        public AudioPlayer()
        {
            // Retrieve the directory where the project is currently running
            string project_location = AppDomain.CurrentDomain.BaseDirectory;

            // Print the project directory to the console for verification
            Console.WriteLine(project_location);

            // Replace "bin\\Debug\\" in the directory path to get the project's base folder
            string updated_path = project_location.Replace("bin\\Debug\\", "");

            // Combine the updated base path with the audio file name to get the full file path
            string full_path = Path.Combine(updated_path, "playvoice.wav");

            // Call the method to play the audio using the full file path
            play_audio(full_path);
        } // End of constructor

        // Method to play the audio file specified by the full file path
        private void play_audio(string full_path)
        {
            try
            {
                // Create a SoundPlayer instance to load and play the audio file
                using (SoundPlayer player = new SoundPlayer(full_path))
                {
                    // Play the audio synchronously (wait until the audio finishes playing)
                    player.PlaySync();
                }
            }
            catch (Exception error)
            {
                // Print the error message to the console if there's an issue playing the audio
                Console.WriteLine(error.Message);
            } // End of try-catch block
        } // End of 'play_audio' method
    } // End of 'voice_greeting' class
} // End of namespace

