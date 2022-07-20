using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Shared.Domain.SeedWork
{
    /// <summary>
    /// Class that represent and identifies value objects on domains or services.
    /// <note type="note">The value object will always be a model, but it will not always be an entity.</note>
    /// </summary>
    /// <typeparam name="TModel">The type of the model used in the instance of the value object.</typeparam>
    [Serializable]
    public abstract class ValueObject<TModel> : IEquatable<TModel>, IModel
        where TModel : ValueObject<TModel>
    {
        private IList<PropertyInfo> RegisteredProperties { get; set; } = new List<PropertyInfo>();

        /// <summary>
        /// Override of the method that indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this object.</param>
        /// <returns>
        /// True if the current object is equal to the other parameter; otherwise, false.
        /// <para>If obj is null, the method returns false.</para></returns>
        /// <example>
        /// Foo class used in this example.
        /// <para>Here is a sample of how to register properties.</para>
        /// <code>
        /// public class Foo : ValueObject<![CDATA[<Foo>]]>
        /// {
        ///     public Foo(string code, string name, string description)
        ///         : this()
        ///     {
        ///         Code = code;
        ///         Name = name;
        ///         Description = description;
        ///     }
        /// 
        ///     internal Foo() => RegisterProperty(it => it.Code);
        /// 
        ///     public string Code { get; private set; }
        ///     public string Name { get; private set; }
        ///     public string Description { get; private set; }
        /// }
        /// </code>
        /// Bar class used in this example.
        /// <code>
        /// public class Bar
        /// {
        ///     public Bar(string code, string name, string description)
        ///     {
        ///         Code = code;
        ///         Name = name;
        ///         Description = description;
        ///     }
        /// 
        ///     public string Code { get; private set; }
        ///     public string Name { get; private set; }
        ///     public string Description { get; private set; }
        /// }
        /// </code>
        /// FooBar class used in this example.
        /// <para>Here is a sample of how to compare.</para>
        /// <code>
        /// public class FooBar
        /// {
        ///     public void Compare()
        ///     {
        ///         var foo = new Foo("code", "Smarkets com br", "Company Smarkets com br");
        ///         var bar = new Bar("code", "Smarkets com br", "Company Smarkets com br);
        /// 
        ///         var result = foo.Equals(bar);
        ///     }
        /// }
        /// </code>
        /// Result value is false.
        /// </example>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;

            return Equals(obj as TModel);
        }

        /// <summary>
        /// Override of the method that returns the hash code for this instance.
        /// </summary>
        /// <returns>A new hash code.</returns>
        public override int GetHashCode()
        {
            var hashCode = 31;
            var changeMultiplier = false;

            foreach (var property in GetProperties())
            {
                var value = property.GetValue(this, null);

                if (value != null)
                {
                    hashCode = hashCode * ((changeMultiplier) ? 59 : 114) + value.GetHashCode();
                    changeMultiplier = !changeMultiplier;
                }
                else
                    hashCode = hashCode ^ (1 * 13);
            }

            return hashCode;
        }

        /// <summary>
        /// Virtual method that indicates whether the current object is equal to another object of the same type.
        /// <para>In this comparison is taken the previously determined properties are used and not the hash code.</para>
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// True if the current object is equal to the other parameter; otherwise, false.
        /// <para>If obj is null, the method returns false.</para></returns>
        /// <example>
        /// Bar class used in this example. Run the <see cref="RegisterProperty"/> sample first.
        /// <para>Sample to compare different codes, result is ***false***.</para>
        /// <code>
        /// public class Bar
        /// {
        ///     public void Compare()
        ///     {
        ///         var foo = new Foo("code1", "Smarkets com br", "Company Smarkets com br");
        ///         var newFoo = new Foo("code2", "Smarkets com br", "Company Smarkets com br");
        /// 
        ///         var result = foo.Equals(newFoo);
        ///     }
        /// }
        /// </code>
        /// <para>Sample to compare different codes, result is ***true***.</para>
        /// <code>
        /// public class Bar
        /// {
        ///     public void Compare()
        ///     {
        ///         var foo = new Foo("code1", "Smarkets com br", "Company Smarkets com br");
        ///         var newFoo = new Foo("code1", "Smarkets", "Company Smarkets");
        /// 
        ///         var result = foo.Equals(newFoo);
        ///     }
        /// }
        /// </code>
        /// </example>
        public virtual bool Equals(TModel other)
        {
            if (other is null) return false;

            if (ReferenceEquals(this, other)) return true;

            if (GetType() != other.GetType())
                return false;

            foreach (var property in GetProperties())
            {
                if (null == property.GetValue(this, null)) return null == property.GetValue(other, null);
                if (false == property.GetValue(this, null).Equals(property.GetValue(other, null))) return false;
            }

            return true;
        }

        /// <summary>
        /// Determines which properties should be used when comparing value objects.
        /// <note type="warning">If the properties that are to be used in the comparison are not determined, all properties in the class will be used and this can directly impact the performance.</note>
        /// </summary>
        /// <param name="expression">Represents a strongly typed lambda expression as a data structure in the form of an expression tree. This class cannot be inherited.</param>
        /// <example>
        /// Foo class used in this example.
        /// <para>Here is a sample of how to register properties.</para>
        /// <code>
        /// public class Foo : ValueObject<![CDATA[<Foo>]]>
        /// {
        ///     public Foo(string code, string name, string description)
        ///         : this()
        ///     {
        ///         Code = code;
        ///         Name = name;
        ///         Description = description;
        ///     }
        /// 
        ///     internal Foo() => RegisterProperty(it => it.Code);
        /// 
        ///     public string Code { get; private set; }
        ///     public string Name { get; private set; }
        ///     public string Description { get; private set; }
        /// }
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException">If the expression is null, an exception is thrown with message <c>'The expression for registering properties can not be null.'</c>.</exception>
        /// <exception cref="InvalidOperationException">If not found properties into expression, an exception is thrown with message <c>'Could not register property with last expression.'</c>.</exception>
        protected void RegisterProperty(Expression<Func<TModel, object>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException("The expression for registering properties can not be null.");

            MemberExpression memberExpression;

            if (ExpressionType.Convert == expression.Body.NodeType)
            {
                var body = (UnaryExpression)expression.Body;
                memberExpression = body.Operand as MemberExpression;
            }
            else
            {
                memberExpression = expression.Body as MemberExpression;
            }

            if (memberExpression == null)
                throw new InvalidOperationException("Could not register property with last expression.");

            RegisteredProperties.Add(memberExpression.Member as PropertyInfo);
        }

        /// <summary>
        /// Custom the equal operator.
        /// </summary>
        /// <param name="left">Value object from left.</param>
        /// <param name="right">Value object from right.</param>
        /// <returns>True if left value object equals to right, otherwise, false.</returns>
        public static bool operator ==(ValueObject<TModel> left, ValueObject<TModel> right) => Equals(left, null) ? (Equals(right, null)) : left.Equals(right);

        /// <summary>
        /// Custom the not equal operator.
        /// </summary>
        /// <param name="left">Value object from left.</param>
        /// <param name="right">Value object from right.</param>
        /// <returns>True if left value object not equals to right, otherwise, false.</returns>
        public static bool operator !=(ValueObject<TModel> left, ValueObject<TModel> right) => !(left == right);

        private IEnumerable<PropertyInfo> GetProperties()
        {
            if (RegisteredProperties.Any())
                return RegisteredProperties;

            return GetType().GetProperties();
        }
    }
}
