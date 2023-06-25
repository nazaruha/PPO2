import { Link, useNavigate } from "react-router-dom";
import { useFormik } from "formik";
import classNames from "classnames";
import { IProjectCreateItem } from "../types";
import { ProjectCreateSchema } from "../validation";
import http from "../../../../http_common";
import { useState } from "react";
import { ICustomerItem } from "../../Customer/types";

const CreateProject = () => {
    const navigate = useNavigate();

    const init: IProjectCreateItem = {
        name: ""
    };

    const onSubmit = async (values: IProjectCreateItem) => {
        console.log("Create Category values", values);

        http.post("api/project/create", values)
            .then(resp => {
                const { data } = resp;
                console.log("Create Category success", data);
                if (data.success === false) {
                    setFieldError("name", "Такий проект вже існує");
                    return;
                }
                navigate("..");
            }).catch(err => {
                console.log(err);
            })
    }

    const formik = useFormik({
        initialValues: init,
        onSubmit: onSubmit,
        validationSchema: ProjectCreateSchema
    })

    const { errors, values, touched, handleChange, handleSubmit, setFieldValue, setFieldError } = formik;

    return (
        <>
            <h1 className="text-center">Створення Проекту</h1>

            <form onSubmit={handleSubmit} className="col-md-6 offset-md-3">
                <div className="mb-3">
                    <label htmlFor="name" className="form-label">Назва</label>
                    <input id="name" name="name" className={classNames(
                        "form-control",
                        { "is-invalid": errors.name && touched.name }
                    )} value={values.name} onChange={handleChange} placeholder="Введіть назву" />
                    {errors.name && touched.name && (
                        <div className="invalid-feedback">
                            {errors.name}
                        </div>
                    )}
                </div>
                <div className="container d-flex flex-row justify-content-center gap-3">
                    <button type="submit" className="btn btn-primary">Створити</button>
                    <Link to="/" className="btn btn-warning">Відхілити</Link>
                </div>
            </form>
        </>
    )
}

export default CreateProject;