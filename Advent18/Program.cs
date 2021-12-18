using System.Diagnostics;
using System.Text;

Stopwatch watch = new Stopwatch();
watch.Start();
var lines = File.ReadAllLines(args[0]).ToList();
var addAll = lines.Select(line => Parse(line)).Aggregate((op1, op2) => Add(op1, op2));

var answer1 = GetMag(addAll);

List<Tree> additions = new List<Tree>();
for (int i = 0; i < lines.Count; i++)
{
    for (int j = 0; j < lines.Count; j++)
    {
        if (i != j)
        {
            additions.Add(Add(Parse(lines[i]), Parse(lines[j])));
        }
    }
}
watch.Stop();
Console.WriteLine($"Answer1: {answer1}");
Console.WriteLine($"Answer2: {additions.Max(tree => GetMag(tree))}");
Console.WriteLine($"Elapsed: {watch.ElapsedMilliseconds}ms");

static int GetMag(Tree tree)
{
    if (tree.Left == null && tree.Right == null)
    {
        return tree.Value;
    }
    return 3 * GetMag(tree.Left) + 2 * GetMag(tree.Right);
}


static Tree Add(Tree op1, Tree op2)
{
    Tree tree = new Tree();
    tree.Left = op1;
    tree.Right = op2;
    op1.Parent = tree;
    op1.IsLeft = true;
    op2.Parent = tree;
    op2.IsRight = true;

    //Console.WriteLine($"{op1}");
    //Console.WriteLine($"+ {op2}");

    //Console.WriteLine($"after addition: {tree}");

    while (true)
    {
        if (Explode(tree, 0))
        {
            //Console.WriteLine($"after explode:  {tree}");
            continue;
        }
        if (Split(tree))
        {
            //Console.WriteLine($"after split:    {tree}");
            continue;
        }
        break;
    }

    return tree;
}



static bool Explode(Tree tree, int depth)
{
    if (depth >= 5 && tree.Left == null && tree.Right == null)
    {
        var curr = tree;
        while (curr != null && curr.IsLeft)
        {
            curr = curr.Parent;
        }
        curr = curr?.Parent?.Left;

        while (curr?.Right != null)
        {
            curr = curr.Right;
        }

        if (curr != null)
        {
            curr.Value += tree.Value;
        }

        curr = tree.Parent.Right;
        while (curr != null && curr.IsRight)
        {
            curr = curr.Parent;
        }
        curr = curr?.Parent?.Right;

        while (curr?.Left != null)
        {
            curr = curr.Left;
        }

        if (curr != null)
        {
            curr.Value += tree.Parent.Right.Value;
        }

        tree = tree.Parent;
        tree.Left = null;
        tree.Right = null;
        tree.Value = 0;

        return true;
    }
    else
    {
        if (tree.Left != null && Explode(tree.Left, depth + 1))
        {
            return true;
        }
        if (tree.Right != null)
        {
            return Explode(tree.Right, depth + 1);
        }
        return false;
    }
}

static bool Split(Tree tree)
{
    Stack<Tree> stack = new Stack<Tree>(new[] { tree });
    while (stack.Any())
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

        if (curr.Left == null && curr.Right == null && curr.Value >= 10)
        {
            Tree split1 = new Tree();
            Tree split2 = new Tree();
            split1.IsLeft = true;
            split2.IsRight = true;
            split1.Value = curr.Value / 2;
            split2.Value = (curr.Value + 1) / 2;
            split1.Parent = curr;
            split2.Parent = curr;

            curr.Left = split1;
            curr.Right = split2;
            curr.Value = -1;

            return true;
        }
    }
    return false;
}

static Tree Parse(string s)
{
    Tree root = new Tree();

    Tree currNode = root;
    var enumerator = s.GetEnumerator();
    enumerator.MoveNext();
    do
    {
        if (enumerator.Current == '[')
        {
            Tree child = new Tree();
            child.Parent = currNode;
            child.IsLeft = true;
            currNode.Left = child;
            currNode = child;
            enumerator.MoveNext();
            continue;
        }
        if (Char.IsDigit(enumerator.Current))
        {
            List<char> value = new List<char>();
            do
            {
                value.Add(enumerator.Current);
                enumerator.MoveNext();
            }
            while (enumerator.Current != ',' && enumerator.Current != ']');

            currNode.Value = int.Parse(new String(value.ToArray()));
            currNode = currNode.Parent;
            continue;
        }
        if (enumerator.Current == ',')
        {
            Tree child = new Tree();
            child.Parent = currNode;
            child.IsRight = true;
            currNode.Right = child;
            currNode = child;
            enumerator.MoveNext();
            continue;
        }
        if (enumerator.Current == ']')
        {
            currNode = currNode.Parent;
            if (!enumerator.MoveNext())
            {
                break;
            }
            continue;
        }
    }
    while (true);

    return root;
}

public class Tree
{
    public int Value { get; set; }
    public Tree Parent { get; set; }
    public Tree Left { get; set; }
    public Tree Right { get; set; }
    public bool IsLeft { get; set; }
    public bool IsRight { get; set; }
    public override string ToString()
    {
        if (this.Left == null && this.Right == null)
        {
            return this.Value.ToString();
        }
        List<string> strings = new List<string>();
        if (this.Left != null)
        {
            strings.Add("[");
            strings.Add(this.Left.ToString());
            strings.Add(",");
        }
        if (this.Right != null)
        {
            strings.Add(this.Right.ToString());
            strings.Add("]");
        }

        return strings.Aggregate(new StringBuilder(), (sb, str) => sb.Append(str), sb => sb.ToString());
    }
}
