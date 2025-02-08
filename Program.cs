using System.Text.Json.Nodes;
namespace NotePad
{
    class NotePadClass
    {
        /// <summary>
        /// List all notes in the notes.json file
        /// </summary>
        /// <exception cref="Exception"></exception>
        static void ListAllNoteTitles()
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
                    Console.WriteLine(note["Title"]);
                }
            }
        }

        /// <summary>
        /// Add a note to the notes.json file
        /// </summary>
        /// <exception cref="Exception"></exception>
        static void AddNote()
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
                ["Title"] = title,
                ["Content"] = content,
                ["CreatedOn"] = DateTime.Now
            };
            notesArray.Add(newNote);
            File.WriteAllText("notes.json", notesArray.ToString());
        }

        /// <summary>
        /// Run the main method if no note has been created in the past 24 hours.
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
        
        static void PrintHelp()
        {
            Console.WriteLine("----------HELP----------\n");
            Console.WriteLine("By entering 0, you can list all note titles.\nBy entering 1, you can add a note.\nBy entering 2, you can delete a note.\nBy entering 3, you can exit the program.\n");
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
                Console.WriteLine("----------Welcome to NotePad----------\n");
                Console.WriteLine("0: List all note titles\n1: Add a note\n2: Delete a note\n3: Exit\n4: Help\n");
                Console.WriteLine("Enter selection:");
                var selection = Console.ReadLine()?.ToString() ?? string.Empty;
                if (selection is null || selection == string.Empty)
                {
                    Console.WriteLine("Invalid selection");
                    return;
                }
                if (selection != "0" && selection != "1" && selection != "2" && selection != "3" && selection != "4")
                {
                    Console.WriteLine("Invalid selection");
                    return;
                }
                switch (selection)
                {
                    case "0":
                        ListAllNoteTitles();
                        break;
                    case "1":
                        AddNote();
                        break;
                    case "2":
                        DeleteNote();
                        break;
                    case "3":
                        Console.WriteLine("Exiting...");
                        Environment.Exit(0);
                        break;
                    case "4":
                        PrintHelp();
                        break;
                    default:
                        Console.WriteLine("Invalid selection");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Message:\n{e.Message}" +
                                  $"StackTrace:\n{e.StackTrace}");
            }
            // }
        }
    }
}