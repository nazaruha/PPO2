import { Link, useNavigate } from "react-router-dom";
import { IManufacturerCreateItem } from "../types";
import { useFormik } from "formik";
import { ManufacturerCreateSchema } from "../validation";
import classNames from "classnames";
import http from "../../../../http_common";

const ManufacturerCreatePage = () => {
    const navigate = useNavigate();

    const init: IManufacturerCreateItem = {
        name: ""
    };

    const onCreateSubmit = (values: IManufacturerCreateItem) => {
        console.log("VALUES", values);

        http.post("api/manufacturer/create", values)
            .then(resp => {
                console.log("RESP", resp);
                navigate("..");
            })
            .catch(err => {
                console.log("ERR", err);
                const { data } = err.response;
                console.log("ERR", data);
                if (data.message.includes("Назва")) setFieldError("name", data.message);
            })
    }

    const formik = useFormik({
        initialValues: init,
        onSubmit: onCreateSubmit,
        validationSchema: ManufacturerCreateSchema
    })

    const { values, errors, touched, handleChange, handleSubmit, setFieldError } = formik


    return (
        <>
            <h1 className="text-center">Створення Виробника</h1>

            <form onSubmit={handleSubmit} className="col-md-6 offset-md-3">
                <div className="mb-3">
                    <label htmlFor="name" className="form-label">Назва <i className="fa fa-snowflake-o text-danger fa-2xs" aria-hidden="true"></i></label>
                    <input type="text" id="name" name="name" className={classNames(
                        "form-control",
                        { "is-invalid": errors.name && touched.name }
                    )} value={values.name} onChange={handleChange} placeholder="Введіть ім'я" />
                    {errors.name && touched.name && (
                        <div className="invalid-feedback">
                            {errors.name}
                        </div>
                    )}
                </div>

                <div className="container d-flex flex-row justify-content-center gap-3">
                    <button type="submit" className="btn btn-primary">Створити</button>
                    <Link to="../" className="btn btn-warning">Відхілити</Link>
                </div>
            </form>
        </>
    )
}

export default ManufacturerCreatePage;