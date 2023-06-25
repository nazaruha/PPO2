import { useEffect, useState } from "react";
import { Link, useSearchParams } from "react-router-dom";
import { IManufacturerSearch, IManufacturerSearchResult } from "../types";
import http from "../../../../http_common";
import classNames from "classnames";
import ModalDelete from "../../../common/ModalDelete";
import { useFormik } from "formik";

const ManufacturerListPage = () => {
    const [searchParams, setSearchParams] = useSearchParams();
    const [search, setSearch] = useState<IManufacturerSearch>({
        name: searchParams.get("name") || "",
        page: searchParams.get("page") || 1
    });

    const onSubmitSearch = (values: IManufacturerSearch) => {
        console.log("Проводимо пошук", values);
        setSearch(values);
    }

    const formik = useFormik({
        initialValues: search,
        onSubmit: onSubmitSearch
    });

    const { values, touched, errors, handleSubmit, handleChange } = formik;

    const [searchResult, setSearchResult] = useState<IManufacturerSearchResult>({
        manufacturers: [],
        pages: 0,
        currentPage: 0,
        total: 0,
    })

    useEffect(() => {
        http.get<IManufacturerSearchResult>("api/manufacturer/search", {
            params: search
        })
            .then(resp => {
                const { data } = resp;
                console.log("RESP DATA", data);
                setSearchResult(data);

            }).catch(err => {
                console.log("ERR", err);
            })
    }, [search])

    const { manufacturers, pages, currentPage } = searchResult;

    const buttons = [];
    for (let i = 1; i <= pages; i++) {
        buttons.push(i);
    }

    const pagination = buttons.map(x => {
        return (
            <li className={classNames(
                "page-item",
                { "active": x === currentPage }
            )} key={x}>
                <Link className="page-link" to={`?page=${x}`} onClick={() => setSearch({ ...search, page: x })}>
                    {x}
                </Link>
            </li>
        );
    })

    const onDeleteManufacturer = async (id: number) => {
        try {
            await http.delete(`api/manufacturer/delete/${id}`);
            setSearch({ ...search, page: 1 });
        } catch (err) {
            console.log("Delete manufacturer ERR", err);
        }
    }

    const tableViewList = manufacturers.map((item) => {
        return (
            <tr key={item.id}>
                <th scope="row">{item.id}</th>
                <td>{item.name}</td>
                <td style={{ display: 'flex' }}>
                    <Link to={`edit/${item.id}`} className="btn btn-warning"
                    >
                        Змінити <i className="fa fa-pencil" aria-hidden="true"></i>
                    </Link>
                    &nbsp;
                    &nbsp;
                    &nbsp;
                    <ModalDelete
                        id={item.id}
                        text={`Ви дійсно бажаєте видалити виробника '${item.name}'?`}
                        deleteFunc={onDeleteManufacturer}
                    />
                </td>
            </tr>
        )
    })

    return (
        <>
            <h1 className="text-center">Виробники</h1>

            <div className="row mb-3">
                <div className="col-md-8 mb-3">
                    <Link to="create" className="btn btn-success fs-5">Додати <i className="fa fa-plus-square-o fs-6" aria-hidden="true"></i></Link>
                </div>
                <div className="col">
                    <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'row' }}>
                        <label htmlFor="name" className="form-label" style={{ display: 'flex', alignSelf: 'center' }}>Назва</label>
                        <div className="mb-3 ms-2">
                            <input
                                type="text"
                                id="name"
                                name="name"
                                value={values.name}
                                placeholder="Пошук по назві"
                                className={classNames(
                                    "form-control",
                                    { "is-invalid": errors.name && touched.name }
                                )}
                                onChange={handleChange}
                            />
                        </div>
                        <button type='submit' className='btn btn-primary ms-2' style={{ height: '80%' }}>
                            Пошук
                        </button>
                    </form>
                </div>
            </div>

            <table className="table fs-5">
                <thead>
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">Назва</th>
                        <th style={{ width: '20%' }}></th>
                    </tr>
                </thead>
                <tbody>
                    {tableViewList}
                </tbody>
            </table>

            <nav aria-label="..." style={{ display: 'flex', justifyContent: 'end' }}>
                <ul className="pagination">
                    <li className={classNames(
                        "page-item",
                        { "disabled": search.page == 1 }
                    )}>
                        <span className="page-link">
                            <Link to={`?page=${Number(search.page) - 1}`} onClick={() => setSearch({ ...search, page: Number(search.page) - 1 })} className="fa fa-arrow-left text-primary" style={{ textDecoration: "none" }} aria-hidden="true"></Link>
                        </span>
                    </li>
                    {pagination}
                    <li className={classNames(
                        "page-item",
                        { "disabled": search.page == buttons.length }
                    )}>
                        <span className="page-link">
                            <Link to={`?page=${Number(search.page) + 1}`} onClick={() => setSearch({ ...search, page: Number(search.page) + 1 })} className="fa fa-arrow-right text-primary" style={{ textDecoration: "none" }} aria-hidden="true"></Link>
                        </span>
                    </li>
                </ul>
            </nav>
        </>
    )
}

export default ManufacturerListPage;