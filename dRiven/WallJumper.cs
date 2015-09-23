namespace dRiven
{
    using EloBuddy;
    using EloBuddy.SDK;

    using SharpDX;

    internal class WallJumper
    {
        public static void DoWallJump()
        {
            if (Program.QCount != 2)
            {
                return;
            }

            var point = VectorHelper.GetFirstWallPoint(Program.Player.ServerPosition, Game.CursorPos, 20);

            if (point == null)
            {
                return;
            }

            if (Program.E.IsReady())
            {
                Program.E.Cast((Vector3) point);
            }
            else
            {
                Player.IssueOrder(
                    GameObjectOrder.MoveTo,
                    (Vector3)Program.Player.ServerPosition.Extend((Vector3)point, Program.Player.BoundingRadius + 20));
            }

            Core.DelayAction(() => Program.Q.Cast((Vector3)point), 500);
        }
    }

    internal class VectorHelper
    {
        public static Vector2? GetFirstWallPoint(Vector3 from, Vector3 to, float step = 25)
        {
            return GetFirstWallPoint(from.To2D(), to.To2D(), step);
        }

        public static Vector2? GetFirstWallPoint(Vector2 from, Vector2 to, float step = 25)
        {
            var direction = (to - from).Normalized();

            for (float d = 0; d < from.Distance(to); d = d + step)
            {
                var testPoint = from + d*direction;
                var flags = NavMesh.GetCollisionFlags(testPoint.X, testPoint.Y);
                if (flags.HasFlag(CollisionFlags.Wall) || flags.HasFlag(CollisionFlags.Building))
                {
                    return from + (d - step)*direction;
                }
            }

            return null;
        }
    }
}