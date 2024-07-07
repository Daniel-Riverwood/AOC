# AdventOfCode 2023

![CI](https://github.com/eduherminio/AdventOfCode.Template/workflows/CI/badge.svg)

Advent of Code project based on [AoCHelper](https://github.com/eduherminio/AoCHelper) template.

It allows you to focus on solving AoC puzzles while providing you with some performance stats.

Output example:

![aochelper](https://user-images.githubusercontent.com/11148519/142051856-16d9d5bf-885c-44cd-94ae-6f678bcbc04f.gif)

## Basic usage

- Place input files under `Inputs/` dir, following `XX.txt` convention.
- Read the input content from `InputFilePath` and solve the puzzle by implementing `Solve_1()` and `Solve_2()`!

**By default, only your last problem will be solved when running the project**. You can change that by behavior by modifying `Program.cs`.

Invoking **different methods**:

- `Solver.SolveAll();` → solves all the days.

- `Solver.SolveLast();` → solves only the last day.

- `Solver.Solve<Day_XX>();` → solves only day `XX`.

- `Solver.Solve(new uint[] { XX, YY });` → solves only days `XX` and `YY`.

- `Solver.Solve(new [] { typeof(Day_XX), typeof(Day_YY) });` → same as above.

## Notes
- Minimal modifications were made from the original solutions that were applied for each day, to fit into the template (The Original Folder contains the original approaches, each of which were setup as a separate solution).
- Not all the solutions are optimal, but are kept in place as is to show what patterns were being applied on each day.
- A note on the leaderboard below, some times are subject to estimates, as not full focus was on the AoC during that time (each day's problem came out at 7am, with normal workday and meetings)
- Please note that the input's for each day are not included, to get the inputs, please head over to [Advent Of Code](https://adventofcode.com) for year 2023

Final Leaderboard standings:

```
      --------Part 1--------   --------Part 2--------
Day       Time   Rank  Score       Time   Rank  Score
 25   01:21:47   1849      0   01:22:18   1589      0
 24   01:52:01   2908      0   03:48:03   1731      0
 23   01:34:56   3244      0   01:54:26   1356      0
 22   03:50:00   4105      0   04:00:53   3446      0
 21   01:51:57   5752      0   03:59:35   1904      0
 20   01:57:17   3443      0   02:37:22   2108      0
 19   00:54:09   3645      0   03:03:56   3554      0
 18   00:52:51   2999      0   01:18:29   1592      0
 17   02:25:05   3089      0   03:20:01   3274      0
 16   01:20:15   4611      0   01:25:53   4016      0
 15   00:14:50   4847      0   00:49:49   4283      0
 14   00:45:08   5961      0   05:25:30   9710      0
 13   02:46:27   8671      0   03:06:34   6849      0
 12   01:29:19   5712      0   04:53:10   4619      0
 11   01:13:58   7909      0   02:35:58  10019      0
 10   01:30:03   6274      0   02:28:08   3312      0
  9   00:41:58   7362      0   00:45:22   6621      0
  8   00:18:26   5308      0   01:32:23   7172      0
  7   02:04:27  12024      0   02:37:03  10386      0
  6   00:23:22   7678      0   00:28:26   6800      0
  5   01:17:19   9750      0   07:43:14  15213      0
  4   00:15:37   5877      0   00:24:51   3270      0
  3   02:19:01  13056      0   02:28:38  10437      0
  2   00:19:53   4372      0   00:27:06   4544      0
  1   00:35:32  10745      0   01:07:42   7517      0
```
- May spend some time creating a separate branch to create more optimal solutions, however the main branch will only contain the approaches as they were made on each of the advent days.
