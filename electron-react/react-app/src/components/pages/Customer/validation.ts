import * as yup from 'yup';

const phoneRegExp = /^\d{10,12}$/;

export const CustomerCreateSchema = yup.object().shape({
    firstName: yup.string()
        .max(50, "Максимальна к-ть символів 50")
        .required("Введіть ім'я"),
    secondName: yup.string()
        .max(50, "Максимальна кількість символів 50")
        .required("Введіть фамілію"),
    email: yup.string()
        .max(150, "Максимальні к-ть символів 150")
        .email("Невірна пошта")
        .required("Введіть пошту"),
    phone: yup.string()
        .min(10, "Довжина номеру мусить бути від 10 до 12")
        .max(12, "Максимальна к-ть символів 12")
        .matches(phoneRegExp, "Невірний формат телефону (тільки цифри від 10 до 12 символів).")
        .required("Введіть телефон")
})

export const CustomerEditSchema = yup.object().shape({
    firstName: yup.string()
        .max(50, "Максимальна к-ть символів 50")
        .required("Введіть ім'я"),
    secondName: yup.string()
        .max(50, "Максимальна кількість символів 50")
        .required("Введіть фамілію"),
    email: yup.string()
        .max(150, "Максимальні к-ть символів 150")
        .email("Невірна пошта")
        .required("Введіть пошту"),
    phone: yup.string()
        .min(10, "Довжина номеру мусить бути від 10 до 12")
        .max(12, "Максимальна к-ть символів 12")
        .matches(phoneRegExp, "Невірний формат телефону (тільки цифри від 10 до 12 символів).")
        .required("Введіть телефон")
})