import * as yup from 'yup';

export const OrderCreateSchema = yup.object().shape({
    customerId: yup.number()
        .min(1, "Оберіть клієнта")
        .required("Оберіть клієнта"),
    productId: yup.number()
        .min(1, "Оберіть продукт")
        .required("Оберіть продукт"),
    productQuantity: yup.number()
        .min(1, "Виберіть кількість продуктів (мін. 1)")
        .required("Виберіть кількість продуктів"),
    manufacturerId: yup.number()
        .min(1, "Виберіть виробника для продукту")
        .required("Виберіть виробника для продукту"),
    sellDate: yup.date()
        .required("Оберіть дату продажі")
})

export const OrderEditSchema = yup.object().shape({
    customerId: yup.number()
        .min(1, "Оберіть клієнта")
        .required("Оберіть клієнта"),
    productId: yup.number()
        .min(1, "Оберіть продукт")
        .required("Оберіть продукт"),
    productQuantity: yup.number()
        .min(1, "Виберіть кількість продуктів (мін. 1)")
        .required("Виберіть кількість продуктів"),
    manufacturerId: yup.number()
        .min(1, "Виберіть виробника для продукту")
        .required("Виберіть виробника для продукту"),
    sellDate: yup.date()
        .required("Оберіть дату продажі")
})