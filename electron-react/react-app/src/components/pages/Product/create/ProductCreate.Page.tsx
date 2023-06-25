import { Link, useNavigate } from "react-router-dom";
import { IProductCreateItem } from "../types";
import { useFormik } from "formik";
import { ProductCreateSchema } from "../validation";
import classNames from "classnames";
import { useEffect, useState } from "react";
import { IManufacturerItem, IManufacturerServiceResponse } from "../../Manufacturer/types";
import http from "../../../../http_common";

const ProductCreatePage = () => {
    const navigate = useNavigate();

    const [manufacturers, setManufacturers] = useState<IManufacturerItem[]>([]);
    const init: IProductCreateItem = {
        name: "",
        manufacturerId: 0
    };

    const onCreateSubmit = (values: IProductCreateItem) => {
        console.log("VALUES", values);
        http.post("api/product/create", values)
            .then(resp => {
                console.log("RESP", resp);
                navigate("..");
            })
            .catch(err => {
                console.log("ERR", err);
                const { data } = err.response;
                setFieldError("name", data);
            })
    }

    const formik = useFormik({
        initialValues: init,
        onSubmit: onCreateSubmit,
        validationSchema: ProductCreateSchema
    })

    const { errors, values, touched, handleChange, handleSubmit, setFieldError } = formik;

    useEffect(() => {
        http.get<IManufacturerServiceResponse>("api/manufacturer/index")
            .then(resp => {
                const { payload } = resp.data;
                console.log("MANUFACTURER LIST", resp);
                setManufacturers(payload as IManufacturerItem[]);
            })
            .catch(err => {
                console.log("ERR MANLST", err);
            });
    }, []);


    return (
        <>
            <h1 className="text-center">Створення продукту</h1>

            <form onSubmit={handleSubmit} className="col-md-6 offset-md-3">
                <div className="mb-3">
                    <label htmlFor="name" className="form-label">Назва</label>
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

                <div className="mb-3">
                    <label htmlFor="manufacturerId" className='form-label'>Оберіть Виробника</label>
                    <select
                        className={classNames(
                            "form-select",
                            { "is-invalid": errors.manufacturerId && touched.manufacturerId }
                        )}
                        aria-label="Category select"
                        defaultValue={values.manufacturerId} // Значення, яке міститься в select
                        onChange={handleChange} // Якщо значення міняється, воно записується у формік
                        name="manufacturerId" // Значення поля у форміку = manufacturerId - якщо його не буде - formik - не буде нічого добавлять
                        id='manufacturerId' // Це використовується для label
                    >
                        {/* Значення, яке завжди буде не обране, для того, щоб нагадать, що треба вказувать категорію */}
                        <option value="0" disabled>---</option> {/* disabled - щоб при виборі категорії ти не міг обрати цей варік */}
                        {/* Перебираємо список категорій і виводимо їх у вигляді options */}
                        {manufacturers.map(item => (
                            <option key={item.id} value={item.id}>{item.name}</option>
                        ))}
                    </select>
                    {(errors.manufacturerId && touched.manufacturerId) && (
                        <span className='text-danger'>{errors.manufacturerId}</span>
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

export default ProductCreatePage;