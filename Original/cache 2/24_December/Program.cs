using Utilities;
using Microsoft.Z3;
using static Utilities.Utilities;
using System.Diagnostics;

var GetInput = (string inputFileName) => File.ReadAllText(inputFileName);

var PrintOutput = (string result) => Console.WriteLine(result);

var GenerateCollections = (string lines) => lines.SplitByDoubleNewline();

var CreateStones = (List<string> stones) => 
{
    var hail = new List<Hail>();
    foreach(var stone in stones)
    {
        var st = stone.Split('@');
        var pos = st[0].Trim().Split(',');
        var x = Convert.ToDecimal(pos[0].Trim());
        var y = Convert.ToDecimal(pos[1].Trim());
        var z = Convert.ToDecimal(pos[2].Trim());
        var vec = st[1].Trim().Split(',');
        var vx = Convert.ToDecimal(vec[0].Trim());
        var vy = Convert.ToDecimal(vec[1].Trim());
        var vz = Convert.ToDecimal(vec[2].Trim());

        hail.Add(new Hail(x, y, z, vx, vy, vz));
    }

    return hail;
};

var Actions = (string area, decimal start, decimal end) =>
{
    var stones = CreateStones(area.SplitByNewline().ToList());
    var res = 0;

    for (var x = 0; x < stones.Count; x++)
    {
        for (var y = x + 1; y < stones.Count; y++)
        {
            var a = stones[x];
            var b = stones[y];
            var det = ((b.Vy + b.Y - b.Y) * (a.X + a.Vx - a.X)) - ((b.Vx + b.X - b.X) * (a.Y + a.Vy - a.Y));

            if (det == 0) continue;

            decimal ua = ((b.Vx + b.X - b.X) * (a.Y - b.Y) - (b.Vy + b.Y - b.Y) * (a.X - b.X)) / det;
            //decimal ub = ((b.Vx + b.X - b.X) * (a.Y - b.Y) - (a.Vy + a.Y - a.Y) * (a.X - b.X)) / det;

            var intersect = new Point(a.X + ua * (a.Vx + a.X - a.X), a.Y + ua * (a.Vy + a.Y - a.Y));

            if (intersect.X > a.X == (a.Vx + a.X - a.X) > 0 && intersect.Y > a.Y == (a.Vy + a.Y - a.Y) > 0 && intersect.X > b.X == (b.Vx + b.X - b.X) > 0 && intersect.Y > b.Y == (b.Vy + b.Y - b.Y) > 0
                && intersect.X >= start && intersect.X <= end && intersect.Y >= start && intersect.Y <= end)
            {
                res++;
            }
        }
    }
    return res;
};

var Collide = (string area) =>
{
    var stones = CreateStones(area.SplitByNewline().ToList());
    var res = 0;
    var ctx = new Context();
    Func<string, BitVecExpr> I = (name) => ctx.MkBVConst(name, 64);

    BitVecExpr x = I("x"), y = I("y"), z = I("z");
    BitVecExpr vx = I("vx"), vy = I("vy"), vz = I("vz");
    var solver = ctx.MkSolver();

    for(var r = 0; r < stones.Count; r++)
    {
        var stone = stones[r];
        BitVecExpr t = I($"t_{r}");
        solver.Add(ctx.MkBVSGE(t, ctx.MkBV(0, 64)));
        solver.Add(ctx.MkEq(ctx.MkBVAdd(x, ctx.MkBVMul(vx, t)), ctx.MkBVAdd(ctx.MkBV(Convert.ToInt64(stone.X), 64), ctx.MkBVMul(ctx.MkBV(Convert.ToInt64(stone.Vx), 64), t))));
        solver.Add(ctx.MkEq(ctx.MkBVAdd(y, ctx.MkBVMul(vy, t)), ctx.MkBVAdd(ctx.MkBV(Convert.ToInt64(stone.Y), 64), ctx.MkBVMul(ctx.MkBV(Convert.ToInt64(stone.Vy), 64), t))));
        solver.Add(ctx.MkEq(ctx.MkBVAdd(z, ctx.MkBVMul(vz, t)), ctx.MkBVAdd(ctx.MkBV(Convert.ToInt64(stone.Z), 64), ctx.MkBVMul(ctx.MkBV(Convert.ToInt64(stone.Vz), 64), t))));
    }
    Debug.Assert(solver.Check() == Status.SATISFIABLE);
    Model m = solver.Model;
    x = (BitVecExpr)m.Eval(x);
    y = (BitVecExpr)m.Eval(y);
    z = (BitVecExpr)m.Eval(z);
    return ((BitVecNum)x).Int64 + ((BitVecNum)y).Int64 + ((BitVecNum)z).Int64;
};


var ProcessInputStep1 = (string lines) => Actions(GenerateCollections(lines).First(), 200000000000000, 400000000000000);

var ProcessInputStep2 = (string lines) => Collide(GenerateCollections(lines).First());

var input = GetInput("input.txt");
var CalValues = ProcessInputStep1(input);
var result = "Step 1: " + CalValues;
PrintOutput(result);
var CalValues2 = ProcessInputStep2(input);
result = "Step 2: " + CalValues2;
PrintOutput(result);

public class Point(decimal x, decimal y)
{
    public decimal X { get; set; } = x;
    public decimal Y { get; set; } = y;
}


public class Hail(decimal x, decimal y, decimal z, decimal vx, decimal vy, decimal vz)
{
    public decimal X { get; set; } = x;
    public decimal Y { get; set; } = y;
    public decimal Z { get; set; } = z;
    public decimal Vx { get; set; } = vx;
    public decimal Vy { get; set; } = vy;
    public decimal Vz { get; set; } = vz;
}
