using System.Text.Json.Nodes;
namespace NotePad
{
    class NotePadClass
    {
        /// <summary>
        /// Not used yet. I can use this for the menu selection switch/case.
        /// Program state enum.
        /// </summary>
        enum ProgramMenuState
        {
            ListAllNoteTitles,
            SelectOnlyOneNoteToDisplay,
            AddNote,
            DeleteNote,
            Exit,
            PrintHelp
        }

        /// <summary>
        /// List all notes in the notes.json file
        /// </summary>
        /// <exception cref="Exception"></exception>
        static void ListAllNoteTitles()
        {
            try
            {
                var notes = File.ReadAllText("notes.json");
                var parsedNotes = JsonNode.Parse(notes);
                if (parsedNotes == null || notes == null)
                {

                    throw new Exception("No notes found.");
                }
                var notesArray = parsedNotes.AsArray();
                Console.WriteLine("----------All Note Titles----------");
                foreach (var noteNode in notesArray)
                {
                    if (noteNode is JsonObject note)
                    {
                        Console.WriteLine(note["Title"]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Message:\n{e.Message}" +
                                  $"StackTrace:\n{e.StackTrace}");

                throw new Exception("Error in ListAllNoteTitles");
            }
        }

        /// <summary>
        /// Add a note to the notes.json file
        /// </summary>
        /// <exception cref="Exception"></exception>
        static void AddNote()
        {
            try
            {
                Console.WriteLine("Enter note title:");
                var title = Console.ReadLine();
                Console.WriteLine("Enter note content:");
                var content = Console.ReadLine();
                var notes = File.ReadAllText("notes.json");
                var parsedNotes = JsonNode.Parse(notes);
                if (parsedNotes == null || notes == null)
                {
                    throw new Exception("No notes found.");
                }
                var notesArray = parsedNotes.AsArray();
                var newNote = new JsonObject
                {
                    ["NoteID"] = Guid.NewGuid().ToString(),
                    ["Title"] = title,
                    ["Content"] = content,
                    ["CreatedOn"] = DateTime.Now
                };
                notesArray.Add(newNote);
                File.WriteAllText("notes.json", notesArray.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Message:\n{e.Message}" +
                                  $"StackTrace:\n{e.StackTrace}");

                throw new Exception("Error in AddNote");
            }
        }

        /// <summary>
        /// Run the main method if no note has been created in the past 24 hours.
        /// TODO: 
        //      - Get this working.
        //      - Make sure that this app is set to start on boot.
        /// </summary>
        /// <exception cref="Exception"></exception>
        static void RunMainIfNoNoteInPast24Hours()
        {
            var notes = File.ReadAllText("notes.json");
            var parsedNotes = JsonNode.Parse(notes);
            if (parsedNotes == null || notes == null)
            {
                throw new Exception("No notes found.");
            }
            var notesArray = parsedNotes.AsArray();
            foreach (var noteNode in notesArray)
            {
                if (noteNode is JsonObject note)
                {
                    if (noteNode == null || note == null)
                    {
                        throw new Exception("No notes found.");
                    }
                    DateTime? createdOn = null;
                    if (note["CreatedOn"] is not null && DateTime.TryParse(note["CreatedOn"]?.ToString(), out DateTime parsedDate))
                    {
                        createdOn = parsedDate;
                    }
                    if (createdOn != null && createdOn > DateTime.Now.AddDays(-1))
                    {
                        Main();
                        return;
                    }
                }
            }
            Main();
        }

        /// <summary>
        /// Delete a note from the notes.json file.
        /// </summary>
        /// <exception cref="Exception"></exception>
        static void DeleteNote()
        {
            try
            {
                Console.WriteLine("Enter note title to delete:");
                var title = Console.ReadLine();
                var notes = File.ReadAllText("notes.json");
                var parsedNotes = JsonNode.Parse(notes);
                if (parsedNotes == null || notes == null)
                {
                    throw new Exception("No notes found.");
                }
                var notesArray = parsedNotes.AsArray();
                foreach (var noteNode in notesArray)
                {
                    if (noteNode is JsonObject)
                    {
                        if (noteNode["Title"]?.ToString() == title)
                        {
                            notesArray.Remove(noteNode);
                            File.WriteAllText("notes.json", notesArray.ToString());
                            Console.WriteLine("Note deleted.");
                            return;
                        }
                    }
                }
                Console.WriteLine("Note not found.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Message:\n{e.Message}" +
                                  $"StackTrace:\n{e.StackTrace}");

                throw new Exception("Error in DeleteNote");
            }
        }

        /// <summary>
        /// Select only one note to display.
        /// </summary>
        /// <exception cref="Exception"></exception>
        static void SelectOnlyOneNoteToDisplay()
        {
            try
            {
                Console.WriteLine("Enter note title to display:");
                var title = Console.ReadLine();
                var notes = File.ReadAllText("notes.json");
                var parsedNotes = JsonNode.Parse(notes);
                if (parsedNotes == null || notes == null)
                {
                    throw new Exception("No notes found.");
                }
                var notesArray = parsedNotes.AsArray();
                foreach (var noteNode in notesArray)
                {
                    if (noteNode is JsonObject)
                    {
                        if (noteNode["Title"]?.ToString() == title)
                        {
                            Console.WriteLine($"-----------------{title}-----------------");
                            Console.WriteLine(noteNode["Content"]?.ToString());
                            return;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Message:\n{e.Message}" +
                                  $"StackTrace:\n{e.StackTrace}");

                throw new Exception("Error in SelectOnlyOneNoteToDisplay");
            }
            Console.WriteLine("Note not found.");
        }

        /// <summary>
        /// Print help to the console.
        /// </summary>
        static void PrintHelp()
        {
            try
            {
                Console.WriteLine("----------HELP----------\n");
                Console.WriteLine("By entering 0, you can list all note titles.\nBy entering 1, you can view a note.\nBy entering 2, you can add a note.\nBy entering 3, you can delete a note.\nBy entering 4, you can exit the program.\n");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Message:\n{e.Message}" +
                                  $"StackTrace:\n{e.StackTrace}");

                throw new Exception("Error in PrintHelp");
            }
        }
        static void Main()
        {
            // RunMainIfNoNoteInPast24Hours();
            // if (!File.Exists("notes.json"))
            // {
            //     Console.WriteLine("Creating notes.json file");
            //     File.WriteAllText("notes.json", "[]");
            // }
            // else
            // {
            try
            {
                while (true)
                {
                Console.WriteLine("----------Welcome to NotePad----------\n");
                Console.WriteLine("0: List all note titles\n1: Select a note to view\n2: Add a note\n3: Delete a note\n4: Exit\n5: Help\n");
                Console.WriteLine("Enter selection:");
                var selection = Console.ReadLine()?.ToString() ?? string.Empty;
                if (selection is null || selection == string.Empty || (selection != "0" && selection != "1" && selection != "2" && selection != "3" && selection != "4" && selection != "5"))
                {
                    throw new Exception("Invalid selection");
                }
                foreach (var c in selection)
                {
                    if (!char.IsDigit(c) || char.IsWhiteSpace(c) || char.IsPunctuation(c) || char.IsSeparator(c) || char.IsSymbol(c))
                    {
                        throw new Exception("Invalid selection");
                    }
                }
                switch (selection)
                {
                    case "0":
                        ListAllNoteTitles();
                        continue;
                    case "1":
                        SelectOnlyOneNoteToDisplay();
                        continue;
                    case "2":
                        AddNote();
                        continue;
                    case "3":
                        DeleteNote();
                        continue;
                    case "4":
                        Console.WriteLine("Exiting...");
                        Environment.Exit(0);
                        break;
                    case "5":
                        PrintHelp();
                        continue;
                    default:
                        Console.WriteLine("Invalid selection");
                        break;
                }
            }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Message:\n{e.Message}" +
                                  $"StackTrace:\n{e.StackTrace}");

                throw new Exception("Error in Main");
            }
            // }
        }
    }
}