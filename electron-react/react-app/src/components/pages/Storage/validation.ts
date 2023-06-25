import * as yup from 'yup';

export const StorageCreateSchema = yup.object().shape({
    productId: yup.number()
        .min(1, "Оберіть продукт"),
    manufacturerId: yup.number()
        .min(1, "Оберіть виробника для вибору продукту"),
    price: yup.number()
        .min(1, "Введіть ціну")
        .required("Введіть ціну"),
    count: yup.number()
        .min(1, "Введіть кількість")
        .required("Введіть кількість"),
    description: yup.string()
        .max(255, "Максимальна к-ть символів 255"),
    expireDate: yup.date()
        .required("Оберіть дату терміну придатності")
});


export const StorageEditSchema = yup.object().shape({
    price: yup.number()
        .required("Введіть ціну"),
    count: yup.number()
        .required("Введіть кількість"),
    description: yup.string()
        .max(255, "Максимальна к-ть символів 255"),
    expireDate: yup.date()
        .required("Оберіть дату терміну придатності")
})