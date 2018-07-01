using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using StarFinder;
using System;
using System.Collections;

namespace SessionSeven.Basement
{
    [Serializable]
    public class CarpetWall : Entity
    {
        public CarpetWall()
        {
            Interaction
                .Create(this)
                .SetPosition(198, 246)
                .SetDirection(Directions8.Left)
                .SetGetInteractionsFn(GetInteractions);

            HotspotMesh
                .Create(this)
                .SetCaption("rugs")
                .SetMesh(CreateMesh());

            Enabled = false;
        }

        private Mesh<TriangleVertexData> CreateMesh()
        {
            var Points = new PathVertex[9]
            {
                new PathVertex(84, 84),
                new PathVertex(107, 83),
                new PathVertex(128, 169),
                new PathVertex(161, 222),
                new PathVertex(152, 236),
                new PathVertex(125, 237),
                new PathVertex(94, 183),
                new PathVertex(95, 126),
                new PathVertex(138, 208)
            };

            var Indices = new int[21]
            {
                0, 1, 2, 2, 6, 7,
                0, 2, 7, 4, 6, 8,
                4, 5, 6, 2, 6, 8,
                3, 4, 8
            };

            return new Mesh<TriangleVertexData>(Points, Indices);
        }

        public Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript());

        }

        IEnumerator LookScript()
        {
            yield return Game.Ego.GoTo(this);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.Say("Some of our failed statement rugs.");
                yield return Game.Ego.Say("It took Cynthia almost a year to finally be happy with the design scheme of the house.");
            }
        }
    }
}
