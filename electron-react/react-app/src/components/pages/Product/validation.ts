import * as yup from 'yup';

export const ProductCreateSchema = yup.object().shape({
    name: yup.string()
        .max(50, "Максимальна к-ть символів 50")
        .required("Введіть ім'я"),
    manufacturerId: yup.number()
        .min(1, "Оберіть виробника")
})

export const ProductEditSchema = yup.object().shape({
    name: yup.string()
        .max(50, "Максимальна к-ть символів 50")
        .required("Введіть ім'я"),
    manufacturerId: yup.number()
        .min(1, "Оберіть виробника")
})