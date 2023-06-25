import * as yup from 'yup';

export const ProjectCreateSchema = yup.object().shape({
    name: yup.string()
        .max(50, "Максимальна к-ть символів 50")
        .required("Введіть назву нового проекту")
})