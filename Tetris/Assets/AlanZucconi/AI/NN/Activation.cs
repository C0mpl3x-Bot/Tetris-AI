using System;
using UnityEngine;

namespace AlanZucconi.AI.NN
{
    public abstract class Activation
    {
        public abstract float F(float x); // Function
        public abstract float D(float x); // Derivative

        public static readonly Activation Logistic = new Logistic();
        public static readonly Activation Tanh = new Tanh();
        public static readonly Activation ReLU = new ReLU();
        public static readonly Activation LeakyReLU = new LeakyReLU(0.01f);
    }

    // https://ml-cheatsheet.readthedocs.io/en/latest/activation_functions.html
    public class Logistic : Activation
    {
        public override float F(float x)
        {
            return 1f / (1f + Mathf.Exp(-x));
        }

        public override float D(float x)
        {
            return F(x) * (1f - F(x));
        }
    }

    // Hyperbolic tangent
    public class Tanh : Activation
    {
        public override float F(float x)
        {
            return (Mathf.Exp(+x) - Mathf.Exp(-x)) / (Mathf.Exp(+x) + Mathf.Exp(-x));
        }

        public override float D(float x)
        {
            return 1 - Mathf.Pow(F(x), 2f);
        }
    }

    public class ReLU : Activation
    {
        public override float F(float x)
        {
            return Mathf.Max(0, x);
        }

        public override float D(float x)
        {
            return x > 0 ? 1f : 0f;
        }
    }

    public class LeakyReLU : Activation
    {
        public float Alpha;

        public LeakyReLU (float alpha)
        {
            Alpha = alpha;
        }

        public override float F(float x)
        {
            return Mathf.Max(Alpha*x, x);
        }

        public override float D(float x)
        {
            return x > 0 ? 1f : Alpha;
        }
    }

    // Fires only if the signal is higher than a threshold
    public class Threshold : Activation
    {
        public Activation Activation;
        public float T;

        public Threshold (Activation activation, float t)
        {
            Activation = activation;
            T = t;
        }

        public override float F(float x)
        {
            float ax = Activation.F(x);
            return  ax > T ? ax : 0f;
        }

        public override float D(float x)
        {
            float ax = Activation.F(x);
            return ax > 0 ? Activation.D(x) : 0f;
        }
    }
}
