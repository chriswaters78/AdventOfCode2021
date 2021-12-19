using System.Diagnostics;
using System.Text;

const bool debug = false;

var watch = new Stopwatch();
watch.Start();

var lines = File.ReadAllLines(args[0]).ToList();
var answer1 = lines.Select(line => Tree.Parse(line)).Aggregate((op1, op2) => add(op1, op2)).Magnitude();

var answer2 = (
     from i in Enumerable.Range(0, lines.Count)
     from j in Enumerable.Range(0, lines.Count)
     where i != j
     select add(Tree.Parse(lines[i]), Tree.Parse(lines[j])).Magnitude())
.Max();

watch.Stop();
Console.WriteLine($"Answer1: {answer1}");
Console.WriteLine($"Answer2: {answer2}");
Console.WriteLine($"Elapsed: {watch.ElapsedMilliseconds}ms");


static Tree add(Tree op1, Tree op2)
{
    Tree tree = new Tree();
    tree.Left = op1;
    tree.Right = op2;
    op1.Parent = tree;
    op2.Parent = tree;
    op1.IsLeft = true;
    op2.IsRight = true;

    if (debug)
    {
        Console.WriteLine($"{op1}");
        Console.WriteLine($"+ {op2}");
        Console.WriteLine($"after addition: {tree}");
    }

    while (true)
    {
        if (explode(tree, 0))
        {
            if (debug) Console.WriteLine($"after explode:  {tree}");
            continue;
        }
        if (split(tree))
        {
            if (debug) Console.WriteLine($"after split:    {tree}");
            continue;
        }
        break;
    }

    return tree;
}

static bool explode(Tree tree, int depth)
{
    if (depth == 5)
    {
        foreach ((Tree from, Tree next)  in new[] { (tree, tree.FindLeft()), (tree.Parent.Right,tree.Parent.Right.FindRight()) })
        {                
            if (next != null)
            {
                next.Value += from.Value;
            }
        }
        
        tree = tree.Parent;
        tree.Left = null;
        tree.Right = null;
        tree.Value = 0;

        return true;
    }
    if (tree.Left != null && explode(tree.Left, depth + 1))
    {
        return true;
    }
    if (tree.Right != null)
    {
        return explode(tree.Right, depth + 1);
    }
    return false;
}

static bool split(Tree tree)
{
    Stack<Tree> stack = new Stack<Tree>(new[] { tree });
    while (stack.Count > 0)
    {
        var curr = stack.Pop();
        if (curr.Right != null)
        {
            stack.Push(curr.Right);
        }
        if (curr.Left != null)
        {
            stack.Push(curr.Left);
        }

        if (curr.Value >= 10)
        {
            curr.Left = new Tree(curr, true) 
            {  
                Value = curr.Value / 2
            };
            curr.Right = new Tree(curr, false) 
            { 
                Value = (curr.Value + 1) / 2 
            };
            curr.Value = null;

            return true;
        }
    }
    return false;
}

public class Tree
{
    public Tree() { }
    public Tree(Tree parent, bool isLeft)
    {
        this.Parent = parent;
        this.IsLeft = isLeft;
        this.IsRight = !isLeft;
    }

    public int? Value { get; set; }
    public Tree? Parent { get; set; }
    public Tree? Left { get; set; }
    public Tree? Right { get; set; }
    public bool IsLeft { get; set; }
    public bool IsRight { get; set; }

    public Tree FindLeft() => find(true);
    public Tree FindRight() => find(false);

    private Tree find(bool left)
    {
        var curr = this;
        while (curr != null && ((left && curr.IsLeft) || (!left && curr.IsRight)))
        {
            curr = curr.Parent;
        }
        curr = left ? curr?.Parent?.Left : curr?.Parent?.Right;

        while ((left && curr?.Right != null) || (!left && curr?.Left != null))
        {
            curr = left ? curr.Right : curr.Left;
        }

        return curr;
    }

    public int Magnitude()
    {
        if (this.Left == null && this.Right == null)
        {
            return this.Value.Value;
        }
        return 3 * this.Left.Magnitude() + 2 * this.Right.Magnitude();
    }
    public static Tree Parse(string str)
    {
        Tree root = new Tree();

        Tree currNode = root;
        var enumerator = str.GetEnumerator();
        enumerator.MoveNext();
        while (true)
        {
            switch (enumerator.Current)
            {
                case '[':
                    Tree left = new Tree(currNode, true);
                    currNode.Left = left;
                    currNode = left;
                    enumerator.MoveNext();
                    break;
                case ',':
                    Tree right = new Tree(currNode, false);
                    currNode.Right = right;
                    currNode = right;
                    enumerator.MoveNext();
                    continue;
                    break;
                case ']':
                    currNode = currNode.Parent;
                    if (!enumerator.MoveNext()) goto quit;
                    break;
                case var ch:
                    List<char> value = new List<char>();
                    do
                    {
                        value.Add(enumerator.Current);
                        enumerator.MoveNext();
                    }
                    while (Char.IsDigit(enumerator.Current));

                    currNode.Value = int.Parse(new string(value.ToArray()));
                    currNode = currNode.Parent;
                    break;
            }
        }
    quit:;

        return root;

    }

    public override string ToString()
    {
        if (this.Left == null && this.Right == null)
        {
            return this.Value.ToString();
        }
        StringBuilder sb = new StringBuilder();
        if (this.Left != null)
        {
            sb.Append("[");
            sb.Append(this.Left.ToString());
            sb.Append(",");
        }
        if (this.Right != null)
        {
            sb.Append(this.Right.ToString());
            sb.Append("]");
        }

        return sb.ToString();
    }
}
