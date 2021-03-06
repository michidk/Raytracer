﻿using Raytracer.Types;

namespace Raytracer
{
    public struct RaycastHit
    {
        public bool Hit;
        public Ray Ray;
        public Renderable Object;

        public Vector3D Position;
        public Vector3D Normal;

        public double DistanceToHit;
    }
}