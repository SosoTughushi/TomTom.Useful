using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TomTom.Useful.ExpressionTreeExtensions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<TDestination, TValue>> ReplaceParameter<TSource, TDestination, TValue>
            (this Expression<Func<TSource, TValue>> expression, Expression<Func<TDestination, TSource>> toExtendWith)
        {
            var replacer = new ParameterReplacer(expression.Parameters.First(),toExtendWith.Body);
            var newBody = replacer.Visit(expression.Body);

            var newLambda = Expression.Lambda(newBody, toExtendWith.Parameters.First());
            return (Expression<Func<TDestination, TValue>>)newLambda;
        }

        public static Expression GetPropertyExpression(this string propertyName,Type propertyType, bool getLambda = true, bool convertToObject = false)
        {
            try
            {
                if (string.IsNullOrEmpty(propertyName))
                    return null;

                var param = Expression.Parameter(propertyType, "param");
                Expression property = null;
                foreach (var fieldName in propertyName.Split('.'))
                {
                    property = property == null
                        ? Expression.Property(param, fieldName)
                        : Expression.Property(property, fieldName);
                }

                if (property.Type.IsEnum)
                {
                    property = Expression.Convert(property, typeof(int));
                }
                if (getLambda)
                {
                    if (convertToObject)
                    {
                        property = Expression.Convert(property, typeof(object));
                    }
                    return Expression.Lambda(property, param);
                }

                return property;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(string.Format("Can't create Property Expression for {0}", propertyName), e);
            }
        }

        public static string ExtractPropertyNameFromExpression<T, TFieldType>(this Expression<Func<T, TFieldType>> propertyExpression)
        {

            if (propertyExpression == null) return null;
            if (propertyExpression.Body is MemberExpression)
            {
                return GetMemberName((propertyExpression.Body as MemberExpression));
            }
            if (propertyExpression.Body is UnaryExpression)
            {
                var unaryExpression = propertyExpression.Body as UnaryExpression;

                if (!(unaryExpression.Operand is MemberExpression))
                    throw new InvalidOperationException(string.Format("can't extract property name from expression specified"));
                return GetMemberName((unaryExpression.Operand as MemberExpression));

            }
            if (propertyExpression.Body is BinaryExpression)
            {
                var binaryBody = (propertyExpression.Body as BinaryExpression);
                return GetMemberName(binaryBody.Left as MemberExpression);
            }

            if (propertyExpression.Body is ParameterExpression)
                return "";

            throw new InvalidOperationException(string.Format("can't extract property name from expression specified"));
        }

        public static Type GetPropertyTypeFromExpression<T>(this Expression<Func<T, object>> propertyExpression)
        {
            var body = propertyExpression.Body;
            if (body is UnaryExpression)
            {
                body = (body as UnaryExpression).Operand;
            }

            return (body).Type;
        }

        public static string ExtractPropertyNameFromMemberExpression(this MemberExpression memberExpression)
        {
            return GetMemberName(memberExpression);
        }

        private static string GetMemberName(MemberExpression memberExpression)
        {
            var member = memberExpression.Member;
            if (member.MemberType != MemberTypes.Property)
            {
                throw new InvalidOperationException(string.Format("{0} is not Property", member.Name));
            }
            string prefix = "";
            if (memberExpression.Expression is MemberExpression)
                prefix = GetMemberName(memberExpression.Expression as MemberExpression) + ".";
            return prefix + member.Name;
        }


        internal class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression parameter;
            private readonly Expression toReplaceWith;

            public ParameterReplacer(ParameterExpression parameter, Expression toReplaceWith)
            {
                this.parameter = parameter;
                this.toReplaceWith = toReplaceWith;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return toReplaceWith;
            }
        }
    }
}
