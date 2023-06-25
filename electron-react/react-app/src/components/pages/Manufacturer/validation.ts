import * as yup from 'yup';

export const ManufacturerCreateSchema = yup.object().shape({
    name: yup.string()
        .max(50, "Максимальна кількість символів 50")
        .required("Введіть назву")
});

export const ManufacturerEditSchema = yup.object().shape({
    name: yup.string()
        .max(50, "Максимальна кількість символів 50")
        .required("Введіть назву")
})