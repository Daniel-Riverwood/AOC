namespace AdventOfCode;

public class Day10 : BaseDay
{
    private readonly List<string> _input;

    public Day10()
    {
        _input = File.ReadAllText(InputFilePath).Split("\n").ToList();
    }
    private Maze GenMaze (List<string> lines)
    {
        var Cur = 'S';
        var maze = lines.Select(l => l.Select(k => '.').ToArray()).ToArray();

        var line = lines.Where(q => q.Contains('S')).First();
        var linepos = lines.IndexOf(line);
        var pos = line.IndexOf('S');
        var res = "";
        var looped = false;
        var UpDown = false; // False is Up, True is Down
        var FB = false; // False is forwards, True is backwards
        while (!looped)
        {
            maze[linepos][pos] = Cur;
            res += Cur;
            var nextLine = "";
            var prevLine = "";
            switch (Cur)
            {
                case 'S':
                    if (res.Where(q => q == 'S').Count() > 1) looped = true;
                    else if (line[pos + 1] == '7' || line[pos + 1] == 'J' || line[pos + 1] == '-')
                    {
                        Cur = line[pos + 1];
                        pos = pos + 1;
                        UpDown = Cur == '7' ? true : (Cur == 'J' ? false : UpDown);
                    }
                    else
                    {
                        if (linepos != 0)
                        {
                            prevLine = lines[linepos - 1];
                            nextLine = lines[linepos + 1];
                            var prevChar = prevLine[pos];
                            var nextChar = nextLine[pos];
                            if (nextChar == '|')
                            {
                                Cur = nextChar;
                                linepos++;
                                UpDown = true;
                            }
                            else if (prevChar == '|')
                            {
                                Cur = prevChar;
                                linepos--;
                                UpDown = false;
                            }
                            else looped = true;
                        }
                        else
                        {
                            nextLine = lines[linepos + 1];
                            var nextChar = nextLine[pos];
                            if (nextChar == '|')
                            {
                                Cur = nextChar;
                                linepos++;
                                UpDown = true;
                            }
                            else looped = true;
                        }
                    }
                    break;
                case 'F':
                    if (!UpDown)
                    {
                        Cur = line[pos + 1];
                        pos++;
                        FB = false;
                        if (Cur == '7') UpDown = true;
                    }
                    else
                    {
                        nextLine = lines[linepos + 1];
                        linepos++;
                        Cur = nextLine[pos];
                        line = nextLine;
                    }
                    break;
                case '7':
                    if (!UpDown)
                    {
                        Cur = line[pos - 1];
                        pos--;
                        FB = true;
                        if (Cur == 'F') UpDown = true;
                    }
                    else
                    {
                        nextLine = lines[linepos + 1];
                        linepos++;
                        Cur = nextLine[pos];
                        line = nextLine;
                    }
                    break;
                case 'J':
                    if (UpDown)
                    {
                        Cur = line[pos - 1];
                        pos--;
                        FB = true;
                        if (Cur == 'L') UpDown = false;
                    }
                    else
                    {
                        prevLine = lines[linepos - 1];
                        linepos--;
                        Cur = prevLine[pos];
                        line = prevLine;
                    }
                    break;
                case 'L':
                    if (UpDown)
                    {
                        Cur = line[pos + 1];
                        pos++;
                        FB = false;
                        if (Cur == 'J') UpDown = false;
                    }
                    else
                    {
                        prevLine = lines[linepos - 1];
                        linepos--;
                        Cur = prevLine[pos];
                        line = prevLine;
                    }
                    break;
                case '-':
                    if (!FB)
                    {
                        Cur = line[pos + 1];
                        pos++;
                        if (Cur == '7') UpDown = true;
                        if (Cur == 'J') UpDown = false;
                    }
                    else
                    {
                        Cur = line[pos - 1];
                        pos--;
                        if (Cur == 'F') UpDown = true;
                        if (Cur == 'L') UpDown = false;
                    }
                    break;
                case '|':
                    if (UpDown)
                    {
                        nextLine = lines[linepos + 1];
                        linepos++;
                        Cur = nextLine[pos];
                        line = nextLine;
                    }
                    else
                    {
                        prevLine = lines[linepos - 1];
                        linepos--;
                        Cur = prevLine[pos];
                        line = prevLine;
                    }
                    break;
                default:
                    break;
            }
        }

        return new Maze(maze, res);
    }

    private long ProcessInput1 ()
    {
        var res = GenMaze(_input);
        var len = res.StringMaze.Trim('S').Length;
        if (len % 2 == 0)
        {
            return len / 2 + 1;
        }
        else
        {
            return (len + 1) / 2;
        }
    }

    private long ProcessInput2 ()
    {
        var maze = GenMaze(_input);
        var res = 0;
        for (var x = 0; x < maze.CharMaze.Count(); x++)
        {
            var isInside = false;
            var linechar = '.';
            for (int y = 0; y < maze.CharMaze[x].Length; y++)
            {
                char cur = maze.CharMaze[x][y];
                if ("|JLF7".Contains(cur))
                {
                    switch (cur)
                    {
                        case 'F':
                            linechar = 'F';
                            break;
                        case '7':
                            if (linechar == 'L') isInside = !isInside;
                            break;
                        case 'J':
                            if (linechar == 'F') isInside = !isInside;
                            break;
                        case 'L':
                            linechar = 'L';
                            break;
                        case '|':
                            isInside = !isInside;
                            break;
                        default:
                            break;
                    }
                }
                else if (cur == '.')
                {
                    if (isInside) res++;
                }
            }
        }

        return res;
    }

    public override ValueTask<string> Solve_1() => new($"{ProcessInput1()}, part 1");

    public override ValueTask<string> Solve_2() => new($"{ProcessInput2()}, part 2");
}
public class Maze(char[][] cmaze, string smaze)
{
    public char[][] CharMaze { get; set; } = cmaze;
    public string StringMaze { get; set; } = smaze;
}